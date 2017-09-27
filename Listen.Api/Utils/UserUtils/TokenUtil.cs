using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Listen.Api.Model;
using Microsoft.AspNetCore.Http;

namespace Listen.Api.Utils.UserUtils
{
    public static class TokenUtil
    {
        public static string GetUserKey(HttpContext context)
        {
            try
            {
                var tokenHeader =
                        ((Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.FrameRequestHeaders)context.Request.Headers)
                        .HeaderAuthorization;
                var token = tokenHeader.ToString().Replace("Bearer", string.Empty).Trim();
                return GetUserKey(token);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string GetUserKey(string token)
        {
            if (string.IsNullOrEmpty(token)) return null;

            var tokenDecoder = new JwtSecurityTokenHandler();
            var jwtSecurityToken = (JwtSecurityToken)tokenDecoder.ReadToken(token);
            return ValidateToken(jwtSecurityToken) ? jwtSecurityToken.Subject : null;
        }


        private static bool ValidateToken(JwtSecurityToken jwtSecurityToken)
        {
            try
            {
                var audience = jwtSecurityToken.Audiences.First();
                var issuer = jwtSecurityToken.Issuer;
                var validTo = jwtSecurityToken.ValidTo;
                var validFrom = jwtSecurityToken.ValidFrom;

                var login = new Login();
                var valid = audience == login.LoginOptions.Audience;
                valid &= issuer == $"https://{login.AuthOptions.Domain}/";
                valid &= validFrom < DateTime.UtcNow;
                valid &= validTo > DateTime.UtcNow;

                return valid;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
