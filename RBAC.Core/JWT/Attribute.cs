using Microsoft.AspNetCore.Mvc;

namespace Security.Core.JWT;

[AttributeUsage(AttributeTargets.Method)]
public class JWTAttribute : TypeFilterAttribute
{
    public JWTAttribute(int appId, string key, string feature, int dRbacLevel, bool bypass = false) : base(typeof(JWTFilter))
    {
        Arguments = [appId, key, feature, dRbacLevel, bypass];
    }

    /// <summary>
    /// JWT attribute to state that the API controller needs authentication
    /// </summary>
    /// <param name="appId">Application identifier</param>
    /// <param name="bypass">[true] to bypass D/RBAC authorization</param>
    public JWTAttribute(int appId, bool bypass) : base(typeof(JWTFilter))
    {
        Arguments = [appId, string.Empty, string.Empty, (int)Common.Core.Enum.DRBACType.DEFAULT, bypass];
    }
}
