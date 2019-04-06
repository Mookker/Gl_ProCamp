using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CommonLibrary.Helpers
{
    public static class JwtGenerator
    {
        /// <summary>
        /// Generates short-lived token for protected controller
        /// </summary>
        /// <returns></returns>
        public static string GenerateProtectedToken(string key, string iss)
        {
            var nbf = DateTime.UtcNow;
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Role, AuthConstants.ProtectedUserRole));

            var tokenString = GenerateTokenString(key, iss, claims, nbf);
            return tokenString;
        }

        private static string GenerateTokenString(string key, string iss, List<Claim> claims, DateTime nbf)
        {
            
            var secBytes = Encoding.UTF8.GetBytes(key);
            var securityKey = new SymmetricSecurityKey(secBytes);
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            
            JwtSecurityToken token = new JwtSecurityToken(
                iss,
                "",
                claims,
                nbf,
                nbf.AddSeconds(300),
                signingCredentials
            );

            var handler = new JwtSecurityTokenHandler();
            var tokenString = handler.WriteToken(token);
            
            return tokenString;
        }

        public static string GenerateUserToken(string key, string iss, string[] roles, string userId)
        {
            var nbf = DateTime.UtcNow;
            var claims = new List<Claim>();
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, userId));
            var tokenString = GenerateTokenString(key, iss, claims, nbf);
            return tokenString; 
        }
    }
}