using Microsoft.IdentityModel.Tokens;
using MovieFinder.Models;
using MovieFinder.Services.Interface;
using MovieFinder.Settings;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieFinder.Services.Implementation
{
    public class TokenService : ITokenService
    {
        public TokenService()
        {
            
        }

        public string CreateJwtToken(Users user)
        {
            Console.WriteLine("**** User logging in: " + user);

            Console.WriteLine("**** jwt secret: " + MoviePrestoSettings.Configuration["JwtSecret"]);

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(MoviePrestoSettings.Configuration["JwtSecret"]));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("Id", user.Id.ToString()),
                    new Claim("UserId", user.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.Add(TimeSpan.Parse(MoviePrestoSettings.Configuration["TokenLifetime"])),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public bool JwtTokenIsValid(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(MoviePrestoSettings.Configuration["JwtSecret"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.Zero
                };
                var princpal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithim(validatedToken))
                {
                    return false;
                }
                return true;

            }
            catch
            {
                return false;
            }
        }

        public RefreshToken CreateRefreshToken(string jwtToken, int userId)
        {
            if (jwtToken == null)
            {
                throw new ArgumentException("JwtToken required to create RefreshToken.");
            }

            if (userId < 0)
            {
                throw new ArgumentException("UserId must be greater than zero.");
            }

            var refreshToken = new RefreshToken()
            {
                Token = Guid.NewGuid().ToString(),
                ExpirationDate = DateTime.UtcNow.AddMonths(int.Parse(MoviePrestoSettings.Configuration["RefreshLifetime"])),
                UserId = userId
            };

            return refreshToken;
        }

        public bool RefreshTokenIsValid(RefreshToken refreshToken)
        {
            try
            {
                if (refreshToken == null)
                {
                    throw new Exception("Refresh token not found");
                }

                if (DateTime.UtcNow > refreshToken.ExpirationDate)
                {
                    throw new Exception("Refresh token has expired");
                }

                if (refreshToken.Invalidated)
                {
                    throw new Exception("Refresh token has been invalidated.");
                }

                return true;
            }

            catch (Exception e)
            {
                Console.WriteLine("Error validating refresh token: " + e.ToString());
                return false;
            }
        }

        /// <summary>
        /// Helper function that validates Jwt token was created with valid security algorithim.
        /// </summary>
        /// <param name="validatedToken"></param>
        /// <returns></returns>
        private bool IsJwtWithValidSecurityAlgorithim(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
