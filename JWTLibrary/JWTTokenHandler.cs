using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JWTLibrary.Model;
using Microsoft.IdentityModel.Tokens;

namespace JWTLibrary;

public class JWTTokenHandler
{
    public const string JWT_SECURITY_KEY = "abcdefgh1234567890abcdefgh1234567890";

    public const int JWT_TOKEN_VALIDITY_MINS = 20;

    private readonly List<UserAccount> userAccounts;

    public JWTTokenHandler()
    {
        userAccounts = new List<UserAccount>
        {
            new UserAccount{UserName = "admin", Password = "admin123", Role = "admin"},
            new UserAccount{UserName = "user", Password = "user123", Role = "user"}
        };
    }

    public AuthResponse GenerateToken(AuthRequest request)
    {
        if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
        {
            return null;
        }
        
        var user = userAccounts.FirstOrDefault(u=>u.UserName == request.UserName && u.Password == request.Password);


        var tokenExpiryTime = DateTime.UtcNow.AddMinutes(JWT_TOKEN_VALIDITY_MINS);
        var tokenKeyInBytes = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);

        var claimsIdentity = new ClaimsIdentity(
            new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role)
            }
        );


        var signingCredentials =
            new SigningCredentials(new SymmetricSecurityKey(tokenKeyInBytes), SecurityAlgorithms.Sha256);

        var securityTokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = claimsIdentity,
            Expires = tokenExpiryTime,
            SigningCredentials = signingCredentials
        };

        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        
        var securitytoken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

        var token = jwtSecurityTokenHandler.WriteToken(securitytoken);
        

        return new AuthResponse
        {
            UserName = user.UserName,
            JwtToken = token,
            ExpiresIn = (int)tokenExpiryTime.Subtract(DateTime.UtcNow).TotalSeconds
        };

    }


}