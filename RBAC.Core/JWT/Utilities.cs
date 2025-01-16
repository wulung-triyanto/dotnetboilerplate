using Common.Core.Constant;
using Common.Core.Extension;
using Common.Core.Function;
using Mapster;
using Security.Core.RBAC;
using System.IdentityModel.Tokens.Jwt;

namespace Security.Core.JWT;

public static class JWTUtilities
{
    public static JwtSecurityToken DecodeJwtToken(this string bearerToken)
    {
        var token = bearerToken.Replace("Bearer ", "");
        var decodedToken = new JwtSecurityToken(token);
        return decodedToken;
    }

    public static string? GetClaimValue(this JwtSecurityToken token, string claimType)
    {
        var claim = token.Claims.FirstOrDefault(c => c.Type == claimType);
        return claim?.Value;
    }

    public static JWTModel GetFieldValue(this JwtSecurityToken token)
    {
        var jwtData = token.Payload.Adapt<JWTModel>();

        jwtData.Data = token.Payload;
        jwtData.ValidFrom = token.ValidFrom;
        jwtData.ValidTo = token.ValidTo;
        jwtData.Roles = token.Claims.Where(c => c.Type == "roles").Select(c => c.Value).ToList();

        return jwtData;
    }

    public static async Task<(bool, string)> Validate(IRBACPersistance redis, string token, CancellationToken cancellationToken)
    {
        JWTModel? jwt = new();

        try
        {
            jwt = token?.DecodeJwtToken().GetFieldValue();
        }
        catch
        {
            return (false, Message.Unauthorized());
        }

        if (jwt == null) { return (false, $"{Message.Unauthorized()} Access token is missing or invalid"); }
        if (jwt.Oid == null) { return (false, $"{Message.Unauthorized} Missing OID."); }
        if (jwt.Roles.IsEmpty()) { return (false, $"{Message.Unauthorized} Missing roles."); }

        var key = $"{CommonConst.ACCESS_TOKEN_SESSION}:{jwt.Oid}:{jwt.Sid}";
        var ttl = await redis.GetTimeToLive(key, cancellationToken);

        if (ttl == null)
        {
            return (false, $"{Message.Unauthorized()} Access token is missing or invalid");
        }

        return (true, "User Authorized.");
    }
}
