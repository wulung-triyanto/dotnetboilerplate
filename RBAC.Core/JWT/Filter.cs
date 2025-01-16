using Common.Core.Constant;
using Common.Core.Enum;
using Common.Core.Extension;
using Common.Core.Model.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Security.Core.RBAC;
using System.Text;

namespace Security.Core.JWT;

public class JWTFilter : IAsyncAuthorizationFilter
{
    #region PRIVATE FIELD
    private readonly int _appId;
    private readonly string _key;
    private readonly string _feature;
    private readonly int _dRbacLevel;
    private readonly bool _bypass;
    private IRBACClient _client;
    private IRBACPersistance _rbacPersistence;
    #endregion

    #region CTOR
    public JWTFilter(int appId, string key, string feature, int dRbacLevel, bool bypass)
    {
        _appId = appId;
        _key = key;
        _feature = feature;
        _dRbacLevel = dRbacLevel;
        _bypass = bypass;
    }
    #endregion

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        JWTModel? jwtData = null;
        var response = new Response();
        var userId = 0;
        var serviceProvider = context.HttpContext.RequestServices;
        var ctx = context.HttpContext?.Items;

        _client = serviceProvider.GetRequiredService<IRBACClient>();
        _rbacPersistence = serviceProvider.GetRequiredService<IRBACPersistance>();

        var transactionId = context.HttpContext?.Request?.Headers?[CommonConst.HEADER_TRANSACTION_ID].FirstOrDefault() ?? string.Empty;
        var bearerAuth = context.HttpContext?.Request?.Headers?[CommonConst.HEADER_AUTHORIZATION].FirstOrDefault() ?? string.Empty;

        try
        {
            jwtData = bearerAuth?.DecodeJwtToken().GetFieldValue();
            int nrp = CommonConst.DEFAULT_NRP;

#if PRD
            if (!string.IsNullOrWhiteSpace(jwtData?.PreferredName.Trim()))
            {
                var regex = new Regex(@"(\d+)@");
                var match = regex.Match(jwtData.PreferredName);

                _ = int.TryParse(match.Groups[1].Value, out nrp);

                nrp = nrp > 0 ? nrp : CommonConst.DEFAULT_NRP;
            }
#endif

            ctx.Add(CommonConst.HEADER_USER_NRP, nrp);
            ctx.Add(CommonConst.HEADER_OID, jwtData?.Oid);
            ctx.Add(CommonConst.HEADER_USER_NAME, jwtData?.Name);
            ctx.Add(CommonConst.HEADER_AUTHORIZATION, bearerAuth);
            ctx.Add(CommonConst.HEADER_USER_ROLE, jwtData?.Roles);
            ctx.Add(CommonConst.HEADER_USER_PREF_NAME, jwtData?.PreferredName);
        }
        catch (Exception e)
        {
            Return401("Unauthorized. Invalid access token");
            return;
        }

        if (jwtData?.Oid == null)
        {
            Return401("Unauthorized. OID in JWT missing");
            return;
        }

        if (jwtData?.Roles.Any() == null)
        {
            Return401("Unauthorized. Role in JWT missing");
            return;
        }

        var key = $"{CommonConst.ACCESS_TOKEN_SESSION}:{jwtData.Oid}:{jwtData.Sid}";
        var ttl = await _rbacPersistence.GetTimeToLiveAsync(key, cancellationToken);

        if (ttl == null)
        {
            Return403("Unauthorized. Access token is missing or invalid");
            return;
        }

        if (_bypass)
        {
            ctx.Add(CommonConst.HEADER_AUTHORIZATION, bearerAuth);
            return;
        }

        #region RBAC
        //RETRIEVE FROM REDIS CACHE
        var rbac = await _rbacPersistence.GetRBACPermissionAsync($"{_key}:{jwtData.Oid}", _feature, cancellationToken);
        var permission = await _rbacPersistence.GetRBACPOAPermissionAsync($"{CommonConst.BASE_RBAC_POA_KEY}:{jwtData.Oid}", _feature, cancellationToken);

        var rbacPoa = permission.Where(x => x.value == (int)PermissionStatus.ACTIVE &&
                                            x.startDate <= DateTime.Now.Date &&
                                            x.endDate >= DateTime.Now.Date).FirstOrDefault();

        if (rbac.PermissionStatus == PermissionStatus.INACTIVE && rbacPoa == null)
        {
            Return403("Access denied, you've got inactive permission");
        }

        userId = rbac.UserId;

        //IF THERE NO DATA, INVOKE API
        if (rbac.PermissionStatus == PermissionStatus.NOFOUND)
        {
            //INVOKE API
            var result = await _client.GetRBACAsync(transactionId, _appId, jwtData.Oid, _feature, cancellationToken);
            if (result?.Status != ResponseStatus.SUCCESS)
            {
                Return403("Access denied, you've got no permission");
            }

            var rbacModel = result?.Data?.FirstOrDefault();
            if (rbacModel?.active == 0)//IF ACTIVE = 0, THEN DENY ACCESS
            {
                Return403("Access denied, you've got no permission to feature");
            }

            userId = rbacModel?.userId ?? 0;
        }
        #endregion

        #region DRBAC
        if ((DRBACType)_dRbacLevel != DRBACType.NONE)
        {
            //RETRIEVE FROM REDIS
            DRBACModel drbac = await _rbacPersistence.GetDRBACPermissionAsync(CommonConst.DRBAC_REDIS_KEY, jwtData.Oid, cancellationToken);
            List<DRBACPOAModel> drbacPOA = await _rbacPersistence.GetDRBACPOAPermissionAsync(CommonConst.DRBAC_POA_REDIS_KEY, jwtData.Oid, cancellationToken);

            //IF THERE NO DATA, INVOKE API
            if (drbac == null)
            {
                var apiResponse = await _client.GetDRBACAsync(transactionId, jwtData.Oid, cancellationToken);

                if (response.Status == ResponseStatus.SUCCESS)
                {
                    drbac = apiResponse.Data;
                }
            }

            if (drbacPOA != null)
            {
                foreach (var poa in drbacPOA)
                {
                    if (poa.startDate <= DateTime.Now.Date && poa.endDate >= DateTime.Now.Date)
                    {
                        var uniqueCompanies = poa.companies.Except(drbac.companies).ToList();
                        var uniqueBusinessUnits = poa.businessUnits.Except(drbac.businessUnits).ToList();
                        var uniqueBranches = poa.branches.Except(drbac.branches).ToList();
                        var uniqueLocations = poa.locations.Except(drbac.locations).ToList();

                        drbac.companies.AddRange(uniqueCompanies);
                        drbac.businessUnits.AddRange(uniqueBusinessUnits);
                        drbac.branches.AddRange(uniqueBranches);
                        drbac.locations.AddRange(uniqueLocations);
                    }
                }
            }

            ctx.Add(CommonConst.DRBAC_PERMISSION_CONTEXT_ITEM_KEY, drbac.Serialize());
            context.HttpContext?.Request.Headers.Remove(CommonConst.HEADER_USER_ID);
            context.HttpContext?.Request.Headers.Add(CommonConst.HEADER_USER_ID, userId.ToString());

            if (_dRbacLevel > 0)
            {
                ctx.Add(CommonConst.DRBAC_LEVEL_CONTEXT_ITEM_KEY, _dRbacLevel);
            }

            if ((DRBACType)_dRbacLevel != DRBACType.DEFAULT)
            {
                //VALIDATE HTTP REQUEST PARAMETER WITH DRBAC
                List<int> ids = [];
                DRBACType level = (DRBACType)_dRbacLevel;
                string method = context.HttpContext?.Request?.Method ?? string.Empty;

                switch (method)
                {
                    case CommonConst.GET:
                        ids.Add(RetrieveIdFromParameter(context, level));
                        break;
                    case CommonConst.PUT:
                        ids = await RetrieveIdFromBody(context, level);
                        break;
                    case CommonConst.POST:
                        ids = await RetrieveIdFromBody(context, level);
                        break;
                    case CommonConst.DELETE:
                        ids.Add(RetrieveIdFromParameter(context, level));
                        break;
                    default:
                        break;
                }

                bool isAuthorized = false;
                switch (level)
                {
                    case DRBACType.DEFAULT:
                        break;
                    case DRBACType.COMPANY:
                        isAuthorized = !ids.Except(drbac.companies).Any();
                        break;
                    case DRBACType.BUSINESSUNIT:
                        isAuthorized = !ids.Except(drbac.businessUnits).Any();
                        break;
                    case DRBACType.BRANCH:
                        isAuthorized = !ids.Except(drbac.branches).Any();
                        break;
                    case DRBACType.LOCATION:
                        isAuthorized = !ids.Except(drbac.locations).Any();
                        break;
                    default:
                        break;
                }

                if (!isAuthorized)
                {
                    Return403("Access denied, you've got no data permission");
                }
            }
        }
        #endregion

        void Return403(string message)
        {
            response.Fail(transactionId, message);
            context.Result = new JsonResult(response)
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }

        void Return401(string message)
        {
            response.Fail(transactionId, message);
            context.Result = new JsonResult(response)
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }
    }

    #region PRIVATE FUNCTION
    /// <summary>
    /// Retrieve id value from route or query string
    /// </summary>
    /// <param name="context"></param>
    /// <param name="drbacLevel"></param>
    /// <returns></returns>
    private int RetrieveIdFromParameter(AuthorizationFilterContext context, DRBACType level)
    {
        int result = 0;

        switch (level)
        {
            case DRBACType.DEFAULT:
                break;
            case DRBACType.COMPANY:
                if (!string.IsNullOrWhiteSpace(context.RouteData.Values["companyId"].ToString()))
                {
                    _ = int.TryParse(context.RouteData.Values["companyId"].ToString(), out result);
                    break;
                }

                if (!string.IsNullOrWhiteSpace(context.HttpContext.Request.Query["companyId"]))
                {
                    _ = int.TryParse(context.HttpContext.Request.Query["companyId"], out result);
                }
                break;
            case DRBACType.BUSINESSUNIT:
                if (!string.IsNullOrWhiteSpace(context.RouteData.Values["businessUnitId"].ToString()))
                {
                    _ = int.TryParse(context.RouteData.Values["businessUnitId"].ToString(), out result);
                    break;
                }

                if (!string.IsNullOrWhiteSpace(context.HttpContext.Request.Query["businessUnitId"]))
                {
                    _ = int.TryParse(context.HttpContext.Request.Query["businessUnitId"], out result);
                }
                break;
            case DRBACType.BRANCH:
                if (!string.IsNullOrWhiteSpace(context.RouteData.Values["branchId"].ToString()))
                {
                    _ = int.TryParse(context.RouteData.Values["branchId"].ToString(), out result);
                    break;
                }

                if (!string.IsNullOrWhiteSpace(context.HttpContext.Request.Query["branchId"]))
                {
                    _ = int.TryParse(context.HttpContext.Request.Query["branchId"], out result);
                }
                break;
            case DRBACType.LOCATION:
                if (!string.IsNullOrWhiteSpace(context.RouteData.Values["locationId"].ToString()))
                {
                    _ = int.TryParse(context.RouteData.Values["locationId"].ToString(), out result);
                    break;
                }

                if (!string.IsNullOrWhiteSpace(context.HttpContext.Request.Query["locationId"]))
                {
                    _ = int.TryParse(context.HttpContext.Request.Query["locationId"], out result);
                }
                break;
            default:
                break;
        }

        return result;
    }

    private async Task<List<int>> RetrieveIdFromBody(AuthorizationFilterContext context, DRBACType level)
    {
        List<int> result = [];
        object tempResult;

        HttpRequestRewindExtensions.EnableBuffering(context.HttpContext.Request);
        var body = context.HttpContext.Request.Body;

        byte[] buffer = new byte[Convert.ToInt32(context.HttpContext.Request.ContentLength)];

        await context.HttpContext.Request.Body.ReadAsync(buffer);

        string requestBody = Encoding.UTF8.GetString(buffer);
        requestBody = requestBody.RemoveLFCR();

        body.Seek(0, SeekOrigin.Begin);

        context.HttpContext.Request.Body = body;

        Dictionary<string, object> drbac = [];
        if (!string.IsNullOrWhiteSpace(requestBody))
        {
            drbac = requestBody.Deserialize<Dictionary<string, object>>();
        }

        switch (level)
        {
            case DRBACType.DEFAULT:
                break;
            case DRBACType.COMPANY:
                result = GetRecursiveId(drbac, "companyId");
                break;
            case DRBACType.BUSINESSUNIT:
                result = GetRecursiveId(drbac, "businessUnitId");
                break;
            case DRBACType.BRANCH:
                result = GetRecursiveId(drbac, "branchId");
                break;
            case DRBACType.LOCATION:
                result = GetRecursiveId(drbac, "locationId");
                break;
            default:
                break;
        }

        return result;
    }

    private List<int> GetRecursiveId(Dictionary<string, object> drbac, string key)
    {
        List<int> result = [];
        int temp;

        foreach (var val in drbac)
        {
            //IF THERE IS NO REQUIRED DRBAC FIELD WITH FIELD NAME = KEY ON REQUEST BODY
            if (!val.Key.IsEqual(key) &&
                (val.Value is not System.Collections.IEnumerable || val.Value is string))// STRING IS IMPLEMENTING IENUMERABLE, SOURCE = "DUDE TRUST ME"
            {
                continue;
            }

            //IF THERE IS FIELD WITH NAME = KEY IN REQUEST BODY
            if (val.Key.IsEqual(key) &&
                val.Value is not System.Collections.IEnumerable &&
                int.TryParse(val.Value.ToString(), out temp))
            {
                result.Add(temp);
                continue;
            }

            //IF THE REQUEST BODY CONTAINS LIST OF OBJECT
            if (val.Value.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>)))
            {
                foreach (var item in ((IEnumerable<object>)val.Value).Cast<object>().ToList())
                {
                    var dict = item as Dictionary<string, object>;

                    if (dict.TryGetValue(key, out object tempId) &&
                        int.TryParse(tempId.ToString(), out temp))
                    {
                        result.Add(temp);
                        continue;
                    }

                    var tempList = GetRecursiveId(dict, key);
                    if (!tempList.IsEmpty())
                    {
                        result.AddRange(tempList);
                    }
                }
            }

            //IF THE REQUEST BODY CONTAIN SINGLE NESTED OBJECT
            if (val.Value.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>)))
            {
                result.AddRange(GetRecursiveId((Dictionary<string, object>)val.Value, key));
            }
        }

        return result.Distinct().ToList();
    }
    #endregion
}