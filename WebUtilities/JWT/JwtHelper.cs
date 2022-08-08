using ExpenseManager.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebUtilities.Exceptions;

namespace WebUtilities.JWT
{
    public class JwtHelper : IJwtHelper
    {
        private readonly SiteSettings _siteSetting;
        public JwtHelper(IOptionsSnapshot<SiteSettings> settings)
        {
            _siteSetting = settings.Value;
        }

        public AccessToken Generate(User user)
        {
            if(_siteSetting == null)
                throw new NotFoundException("Sitesetting is not forund");

            JwtSettings jwtSettings = _siteSetting.JwtSettings;
            if (jwtSettings == null)
                throw new NotFoundException("JWTsetting is not forund");

            var secretKey = Encoding.UTF8.GetBytes(jwtSettings.SecretKey); // longer that 16 character
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);


            var encryptionkey = Encoding.UTF8.GetBytes(jwtSettings.Encryptkey); //must be 16 character
            var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionkey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

            var claims = GetClaims(user);

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtSettings.Issuer,
                Audience = jwtSettings.Audience,
                IssuedAt = DateTime.Now,
                NotBefore = DateTime.Now.AddMinutes(jwtSettings.NotBeforeMinutes),
                Expires = DateTime.Now.AddMinutes(jwtSettings.ExpirationMinutes),
                SigningCredentials = signingCredentials,
                EncryptingCredentials = encryptingCredentials,
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);
            var accessToken = new AccessToken(securityToken);
            return accessToken;
        }

        private IEnumerable<Claim> GetClaims(User user)
        {
            var securityStampClaimType = new ClaimsIdentityOptions().SecurityStampClaimType;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("IsActive", user.IsActive.ToString()),
                new Claim(securityStampClaimType, user.SecurityStamp.ToString()),
            };
            //var roles = new Role[] { new Role { Name = "Admin" } };
            //foreach (var role in roles)
            //    list.Add(new Claim(ClaimTypes.Role, role.Name));

            return claims;
        }
    }
}
