using System.Threading.Tasks;
using GraphQlRethinkDbLibrary;
using GraphQlRethinkDbLibrary.Handlers;
using Listen.Api.Utils.UserUtils;
using Microsoft.AspNetCore.Http;

namespace Listen.Api.Handlers
{
    public class GraphQlHandler<TQuery, TMutation> : GraphQlDefaultHandler<TQuery, TMutation>
    {
        private string _user;

        public override Task Process(HttpContext context)
        {
            _user = TokenUtil.GetUserKey(context);
            return base.Process(context);
        }

        public override UserContext GetUserContext(string body)
        {
            return new UserContext(body, _user);
        }
    }
}
