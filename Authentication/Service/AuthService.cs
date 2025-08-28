using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Authentication.Entity;
using Authentication.Entity.Request;
using Authentication.Repo;
using Microsoft.IdentityModel.Tokens;

namespace Authentication.Service;

public class AuthService
{
    private readonly AuthenticationRepository repository;

    private const string JWT_SECURITY_KEY = "abcdefgh1234567890abcdefgh1234567890";

    private const int JWT_TOKEN_VALIDITY_MINS = 20;
    

    public AuthService(AuthenticationRepository repository)
    {
        this.repository = repository;
    }

    public async Task<bool> Register(User user)
    {
        var isSuccess = await repository.RegisterAsync(user);
        
        return isSuccess;
    }

    public async Task<string> Login(LoginRequest loginRequest)
    {
        // check if username and password is valid
        var user = await repository.LoginAsync(loginRequest.Email, loginRequest.Password);
        
        if (user is null)
            return null!;
        
        // generate jwt
        var jwtToken = await GenerateJwtToken(user);


        return jwtToken;
    }

    private async Task<string> GenerateJwtToken(User user)
    {
        var securtiykeyinBytes = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);

        var tokenExpiryTime = DateTime.UtcNow.AddMinutes(JWT_TOKEN_VALIDITY_MINS);
        
        // define claims identity
        var claimsIdentity = new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role)
        });


        // get the signing key
        var securitySigningKey =
            new SigningCredentials(new SymmetricSecurityKey(securtiykeyinBytes), SecurityAlgorithms.HmacSha256);


        // get the token descriptor
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            Expires = tokenExpiryTime,
            SigningCredentials = securitySigningKey
        };


        // get the token handler
        var tokenHandler = new JwtSecurityTokenHandler();

        var securtyToken = tokenHandler.CreateToken(tokenDescriptor);

        var token = tokenHandler.WriteToken(securtyToken);

        return token;

    }
}