using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace RandomCappuccino.Server.Authentication
{
    public class TokenManager
    {
        private readonly bool validateIssuer;
        private readonly string validIssuer;
        private readonly bool validateAudience;
        private readonly string validAudience;
        private readonly bool validateIssuerSigningKey;
        private readonly SymmetricSecurityKey issuerSigningKey;
        private readonly bool validateLifetime;
        private readonly TimeSpan clockSkew;
        private readonly JwtSecurityTokenHandler tokenHandler;
        
        public TokenManager(
            string issuerSigningKey,
            bool validateIssuer = true,
            string validIssuer = "Issuer",
            bool validateAudience = true,
            string validAudience = "Audience",
            int clockSkewMinutes = 4320,
            bool validateLifetime = true)
        {
            validateIssuerSigningKey = true;
            this.issuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(issuerSigningKey));
            this.validateIssuer = validateIssuer;
            this.validIssuer = validIssuer;
            this.validateAudience = validateAudience;
            this.validAudience = validAudience;
            this.validateLifetime = validateLifetime;
            clockSkew = TimeSpan.FromMinutes(clockSkewMinutes);
            tokenHandler = new JwtSecurityTokenHandler();
        }

        public TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateIssuer = validateIssuer,
                ValidIssuer = validIssuer,
                ValidateAudience = validateAudience,
                ValidAudience = validAudience,
                ValidateIssuerSigningKey = validateIssuerSigningKey,
                IssuerSigningKey = issuerSigningKey,
                ValidateLifetime = validateLifetime,
                ClockSkew = clockSkew
            };
        }
        
        private SecurityTokenDescriptor GetTokenDescriptor(ClaimsIdentity claims)
        {
           return new SecurityTokenDescriptor
           {
               Issuer = validIssuer,
               Audience = validAudience,
               SigningCredentials = new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256Signature),
               Subject = claims,
               Expires = DateTime.UtcNow.Add(clockSkew)
           };
        }

        public string GenerateToken(string userId, IEnumerable<string> userRoles)
        {
            var claims = new ClaimsIdentity("Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            claims.AddClaim(new Claim("UserId", userId));

            foreach (var role in userRoles)
            {
                claims.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, role));
            }            

            var token = tokenHandler.CreateToken(GetTokenDescriptor(claims));

            return tokenHandler.WriteToken(token);
        }

        public string GenerateToken(string userId, params string[] userRoles)
        {
            return GenerateToken(userId, userRoles.AsEnumerable());
        }
    }
}
