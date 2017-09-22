using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using GraphQlRethinkDbLibrary.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Listen.Api.Handlers
{
    public class GraphQlHandler<TQuery, TMutation> : GraphQlDefaultHandler<TQuery, TMutation>
    {
        public override Task Process(HttpContext context)
        {
            var tokenHeader =
                ((Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.FrameRequestHeaders) context.Request.Headers)
                .HeaderAuthorization;
            var token = tokenHeader.ToString().Replace("Bearer",string.Empty).Trim();
            var tokenDecoder = new JwtSecurityTokenHandler();
            var jwtSecurityToken = (JwtSecurityToken)tokenDecoder.ReadToken(token);
            if(!ValidateToken(jwtSecurityToken))
                throw new Exception("Authentication error!");


            return base.Process(context);
        }

        private bool ValidateToken(JwtSecurityToken jwtSecurityToken)
        {
            try
            {
                var subject = jwtSecurityToken.Subject;
                var audience = jwtSecurityToken.Audiences.First();
                var issuer = jwtSecurityToken.Issuer;
                var validTo = jwtSecurityToken.ValidTo;
                var validFrom = jwtSecurityToken.ValidFrom;

                var valid = audience == "https://listen.fredriklowenhamn.se";
                valid &= issuer == "lowet.eu.auth0.com";
                valid &= ValidateUser(subject);
                valid &= validFrom < DateTime.Now;
                valid &= validTo > DateTime.Now;

                return valid;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool ValidateUser(string user)
        {
            return true;
        }
    }
}
