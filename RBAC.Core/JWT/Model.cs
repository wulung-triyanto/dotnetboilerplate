using Mapster;
using System.IdentityModel.Tokens.Jwt;

namespace Security.Core.JWT;

public class JWTModel
{
    public string Aud { get; set; }
    public string Iss { get; set; }
    public long Iat { get; set; }
    public long Nbf { get; set; }
    public long Exp { get; set; }
    public string Aio { get; set; }
    public string Azp { get; set; }
    public long Azpacr { get; set; }
    /// <summary>
    /// User full name
    /// </summary>
    public string Name { get; set; }
    public string Oid { get; set; }
    /// <summary>
    /// User preferred name, usually Azure Active Directory email
    /// </summary>
    [AdaptMember("preferred_name")]
    public string PreferredName { get; set; }
    public string Rh { get; set; }
    /// <summary>
    /// User roles associated with the user
    /// </summary>
    public List<string> Roles { get; set; }
    public string Scp { get; set; }
    public string Sub { get; set; }
    public string Sid { get; set; }
    public string Tid { get; set; }
    public string Uti { get; set; }
    public string Ver { get; set; }
    public JwtPayload Data { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
}
