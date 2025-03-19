using ClubAPI.Extras;
using ClubAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClubAPI.Services
{
    public class JWTServices
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public JWTServices(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<SignInResponse?> Authenticate(SignInRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return null;
            }
            User? user = _dbContext.Users.FirstOrDefault(u => u.Email == request.Email);
            if (user == null)
            {
                return null;
            }
            var res = PasswordHasher.Verify(request.Password, user.Password);
            if (res)
            {
                var issuer = _configuration["JwtConfig:Issuer"];
                var audience = _configuration["JwtConfig:Audience"];
                var key = _configuration["JwtConfig:Key"];
                var validity = Convert.ToInt32(_configuration["JwtConfig:TokenValidityMins"]);
                var tokenExpirity = DateTime.UtcNow.AddMinutes(validity);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(new[]
                    {
                        new Claim("Email", user.Email),
                        new Claim("ID", user.Id.ToString()) // Adding user.id claim
                    }),
                    Expires = tokenExpirity,
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)), SecurityAlgorithms.HmacSha512Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return new SignInResponse
                {
                    Token = tokenString,
                    Email = user.Email,
                    ExpiresIn = validity
                };
            }
            return null;
        }
    }
}
