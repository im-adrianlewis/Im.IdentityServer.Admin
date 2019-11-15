using GraphQL.Types;
using Im.Access.GraphPortal.Repositories;

namespace Im.Access.GraphPortal.Graph.OperationalGroup
{
    public class CircuitBreakerType : ObjectGraphType<CircuitBreakerEntity>
    {
        public CircuitBreakerType()
        {
            Name = "CircuitBreaker";
            Description = "Describes the state of a service circuit breaker";

            Field(e => e.Name).Description("Circuit breaker name");
            Field(e => e.State).Description("State of the circuit breaker");
        }
    }
}