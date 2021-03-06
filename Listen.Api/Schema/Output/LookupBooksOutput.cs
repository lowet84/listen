﻿using GraphQL.Conventions.Relay;

namespace Listen.Api.Schema.Output
{
    public class LookupBooksOutput : IRelayMutationOutputObject
    {
        public int Updated { get; }
        public string ClientMutationId { get; set; }

        public LookupBooksOutput(int updated)
        {
            Updated = updated;
        }
    }


}
