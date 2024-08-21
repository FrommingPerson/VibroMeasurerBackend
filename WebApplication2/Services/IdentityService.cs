using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace WebApplication2.Services;

public class IdentityService
{
    private readonly IConfiguration _configuration;

    public IdentityService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private ClaimsIdentity GetIdentity(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim("Id", user.ApplicationUserId.ToString()),
        };
        var claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
        return claimsIdentity;
    }
    private JwtSecurityToken CreateJwtToken(ClaimsIdentity identity, DateTime expires)
    {
        var jwt = new JwtSecurityToken(
            issuer: _configuration["Identity:ValidIssuer"],
            audience: _configuration["Identity:ValidAudience"],
            notBefore: DateTime.Now,
            claims: identity.Claims,
            expires: expires,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(_configuration["Identity:IssuerSigningKey"])
                ),
                SecurityAlgorithms.HmacSha256
            )
        );
        return jwt;
    }
    private string EncodeJwtToken(JwtSecurityToken jwt)
    {
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        return encodedJwt;
    }

    public string GenerateJwtToken(ApplicationUser user, DateTime expires)
    {
        var claimsIdentity = this.GetIdentity(user);
        var jwtSecurityToken = this.CreateJwtToken(claimsIdentity, expires);
        var jwtEncoded = this.EncodeJwtToken(jwtSecurityToken);
        return jwtEncoded;
    }
}