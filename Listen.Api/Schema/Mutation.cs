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
    }
}
