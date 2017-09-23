using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using GraphQlRethinkDbLibrary;
using GraphQlRethinkDbLibrary.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Listen.Api.Handlers
{
    public class GraphQlHandler<TQuery, TMutation> : GraphQlDefaultHandler<TQuery, TMutation>
    {
        private string _user;

        public override Task Process(HttpContext context)
        {
            var tokenHeader =
                ((Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.FrameRequestHeaders)context.Request.Headers)
                .HeaderAuthorization;
            var token = tokenHeader.ToString().Replace("Bearer", string.Empty).Trim();
            if (!string.IsNullOrEmpty(token))
            {
                var tokenDecoder = new JwtSecurityTokenHandler();
                var jwtSecurityToken = (JwtSecurityToken) tokenDecoder.ReadToken(token);
                if (ValidateToken(jwtSecurityToken))
                    _user = jwtSecurityToken.Subject;
            }
            return base.Process(context);
        }

        public override UserContext GetUserContext(string body)
        {
            return new UserContext(body, _user);
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
                valid &= issuer == "https://lowet.eu.auth0.com/";
                valid &= ValidateUser(subject);
                valid &= validFrom < DateTime.UtcNow;
                valid &= validTo > DateTime.UtcNow;

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
