using GraphQL.Conventions.Relay;

namespace Listen.Api.Schema.Output
{
    public class UpdateFileChangesOutput : IRelayMutationOutputObject
    {
        public int Total { get; }
        public int Added { get; }
        public int Deleted { get; }

        public UpdateFileChangesOutput(int total, int added, int deleted)
        {
            Total = total;
            Added = added;
            Deleted = deleted;
        }

        public string ClientMutationId { get; set; }
    }
}
