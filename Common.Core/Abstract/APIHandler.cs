using Common.Core.Constant;
using Common.Core.Extension;
using Common.Core.Interface.Facade.API;
using Common.Core.Model.Response;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Common.Core.Abstract.API;

/// <summary>
/// Abstract handler for REST API
/// </summary>
/// <param name="facade"></param>
public abstract class AbstractHandler
{
    public readonly IAbstractFacade facade;

    #region CTOR
    protected AbstractHandler() { }

    protected AbstractHandler(IAbstractFacade facade)
    {
        this.facade = facade;
    }
    #endregion

    #region PROPERTIES
    public ClaimsPrincipal CurrentPrincipal
    {
        get
        {
            return facade.httpContext.HttpContext?.User;
        }
    }

    public DRBACModel DRBAC
    {
        get
        {
            DRBACModel drbac = new();
            string? drbacHTTPCtxItem = facade.httpContext.HttpContext?.Items?[CommonConst.HEADER_DRBAC]?.ToString();
            if (!string.IsNullOrWhiteSpace(drbacHTTPCtxItem))
            {
                drbac = drbacHTTPCtxItem?.Deserialize<DRBACModel>();
            }

            return drbac;
        }
    }

    public string BearerToken
    {
        get
        {
            var bearerToken = facade.httpContext.HttpContext?.Request?.Headers?[CommonConst.HEADER_AUTHORIZATION].FirstOrDefault();
            return bearerToken ?? string.Empty;
        }
    }

    public string ClientURL
    {
        get
        {
            var clientURL = facade.httpContext.HttpContext?.Request.GetTypedHeaders().Referer?.ToString();
            return clientURL ?? string.Empty;
        }
    }

    public string Method
    {
        get
        {
            var method = facade.httpContext.HttpContext?.Request?.Method;
            return method;
        }
    }

    public string OID
    {
        get
        {
            var oid = facade.httpContext.HttpContext?.Items?[CommonConst.HEADER_OID]?.ToString();
            return oid ?? string.Empty;
        }
    }

    public string ClientHost
    {
        get
        {
            var clientHost = System.Net.Dns.GetHostEntry(facade.httpContext.HttpContext?.Connection?.RemoteIpAddress)?.HostName;
            return clientHost;
        }
    }

    public string ClientIP
    {
        get
        {
            var clientIP = facade.httpContext.HttpContext?.Request?.Headers?[CommonConst.HEADER_FORWARDED].FirstOrDefault() ??
                           facade.httpContext.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            return clientIP;
        }
    }

    public string SourceURL
    {
        get
        {
            string sourceURL = string.Empty;
            var request = facade.httpContext.HttpContext?.Request;
            if (request != null)
            {
                UriBuilder uriBuilder = new()
                {
                    Scheme = request?.Scheme,
                    Host = request?.Host.Host,
                    Path = request?.Path.ToString(),
                    Query = request?.QueryString.ToString()
                };

                sourceURL = uriBuilder.Uri.ToString();
            }

            return sourceURL;
        }
    }

    protected int TimeOffset
    {
        get
        {
            _ = int.TryParse(facade.httpContext.HttpContext?.Request?.Headers?[CommonConst.HEADER_TIMEOFFSET].FirstOrDefault(), out int timeOffset);
            return timeOffset == 0 ? 7 : timeOffset;
        }
    }

    public string TransactionId
    {
        get
        {
            var transactionId = facade.httpContext.HttpContext?.Request?.Headers?[CommonConst.HEADER_TRANSACTION_ID].FirstOrDefault();
            return transactionId ?? string.Empty;
        }
    }

    public string UserAgent
    {
        get
        {
            var userAgent = facade.httpContext.HttpContext?.Request?.Headers[CommonConst.HEADER_USER_AGENT].FirstOrDefault();
            return userAgent ?? string.Empty;
        }
    }

    public int UserId
    {
        get
        {
            _ = int.TryParse(facade.httpContext.HttpContext?.Request?.Headers?[CommonConst.HEADER_USER_ID].FirstOrDefault(), out int userId);
            return userId;
        }
    }

    /// <summary>
    /// User full name
    /// </summary>
    public string UserName
    {
        get
        {
            var userName = facade.httpContext.HttpContext?.Items?[CommonConst.HEADER_USER_NAME]?.ToString();
            return userName ?? string.Empty;
        }
    }

    public int UserNRP
    {
        get
        {
            _ = int.TryParse(facade.httpContext.HttpContext?.Items?[CommonConst.HEADER_USER_NRP]?.ToString(), out int userNRP);
            return userNRP;
        }
    }

    /// <summary>
    /// User preferred name. Usually user active directory account.
    /// </summary>
    public string UserPreferredName
    {
        get
        {
            var userPrefName = facade.httpContext.HttpContext?.Items?[CommonConst.HEADER_USER_PREF_NAME]?.ToString();
            return userPrefName ?? string.Empty;
        }
    }

    /// <summary>
    /// User roles associated with the user.
    /// </summary>
    public List<string> UserRole
    {
        get
        {
            var userRole = (List<string>)facade.httpContext.HttpContext?.Items?[CommonConst.HEADER_USER_ROLE];
            return userRole;
        }
    }

    /// <summary>
    /// Unique roles associated with the user.
    /// </summary>
    public List<int> UniqueRole
    {
        get
        {
            var uniqueRole = (List<int>)facade.httpContext.HttpContext?.Items?[CommonConst.HEADER_UNIQUE_ROLE];
            return uniqueRole;
        }
    }
    #endregion
}