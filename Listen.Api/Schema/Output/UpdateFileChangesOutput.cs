using GraphQL.Conventions.Relay;
using Listen.Api.Model;

namespace Listen.Api.Schema.Output
{
    public class UpdateFileChangesOutput : IRelayMutationOutputObject
    {
        public Book[] NewBooks { get; }

        public UpdateFileChangesOutput(Book[] newBooks)
        {
            NewBooks = newBooks;
        }

        public string ClientMutationId { get; set; }
    }
}
