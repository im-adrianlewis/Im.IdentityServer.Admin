using GraphQL.Types;
using Im.Access.GraphPortal.Repositories;

namespace Im.Access.GraphPortal.Graph.OperationalGroup
{
    public class ChaosPolicyType : ObjectGraphType<CircuitBreakerPolicyEntity>
    {
        public ChaosPolicyType()
        {
            Name = "ChaosPolicy";
            Description = "Describes the state of a service chaos policy";

            Field(e => e.Name).Description("Circuit breaker name");
            Field(e => e.State).Description("State of the circuit breaker");
        }
    }
}