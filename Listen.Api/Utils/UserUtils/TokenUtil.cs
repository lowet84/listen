using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Listen.Api.Utils.UserUtils
{
    public static class TokenUtil
    {
        public static string GetUserKey(HttpContext context)
        {
            var tokenHeader =
                ((Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.FrameRequestHeaders)context.Request.Headers)
                .HeaderAuthorization;
            var token = tokenHeader.ToString().Replace("Bearer", string.Empty).Trim();
            if (!string.IsNullOrEmpty(token))
            {
                var tokenDecoder = new JwtSecurityTokenHandler();
                var jwtSecurityToken = (JwtSecurityToken)tokenDecoder.ReadToken(token);
                if (ValidateToken(jwtSecurityToken))
                    return jwtSecurityToken.Subject;
            }

            return null;
        }

        private static bool ValidateToken(JwtSecurityToken jwtSecurityToken)
        {
            try
            {
                var audience = jwtSecurityToken.Audiences.First();
                var issuer = jwtSecurityToken.Issuer;
                var validTo = jwtSecurityToken.ValidTo;
                var validFrom = jwtSecurityToken.ValidFrom;

                var valid = audience == "https://listen.fredriklowenhamn.se";
                valid &= issuer == "https://lowet.eu.auth0.com/";
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
