using GraphQL.Types;
using Im.Access.GraphPortal.Repositories;

namespace Im.Access.GraphPortal.Graph.OperationalGroup
{
    public class CircuitBreakerPolicyType : ObjectGraphType<CircuitBreakerPolicyEntity>
    {
        public CircuitBreakerPolicyType()
        {
            Name = "CircuitBreakerPolicy";
            Description = "Describes the state of a service circuit breaker policy";

            Field(e => e.Name).Description("Circuit breaker name");
            Field(e => e.State).Description("State of the circuit breaker");
        }
    }
}