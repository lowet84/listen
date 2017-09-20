using System;
using System.Threading.Tasks;
using GraphQlRethinkDbLibrary;
using GraphQlRethinkDbLibrary.Schema.Output;
using GraphQL.Conventions;
using GraphQL.Conventions.Relay;
using TagLib.Matroska;

namespace Listen.Api.Schema
{
    [ImplementViewer(OperationType.Mutation)]
    public partial class Mutation
    {
        [System.ComponentModel.Description("Clean the database")]
        public DefaultResult<string> Clean(Id? id)
        {
            var key = $"{DateTime.Now.Minute}";
            var keyId = Id.New<string>(key);
            if (id == null)
                return new DefaultResult<string>(keyId.ToString());

            if (!id.GetValueOrDefault().IsIdentifierForType<string>())
                return new DefaultResult<string>("Wrong type of id");

            if (id.ToString() != keyId.ToString())
                return new DefaultResult<string>("Wrong id");

            Task.Run(() => new UserContext().CleanDatabase());
            return new DefaultResult<string>("Cleaning db");
        }
    }
}
