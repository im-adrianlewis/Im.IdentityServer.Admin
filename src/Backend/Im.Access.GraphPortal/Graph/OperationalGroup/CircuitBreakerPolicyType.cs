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

            Field(e => e.Service).Description("Service this policy applies to.");
            Field(e => e.PolicyKey).Description("Policy name for this circuit breaker.");
            Field(e => e.IsIsolated).Description("Current state of the circuit breaker.");
            Field(e => e.LastUpdated).Description("When the state last changed.");
        }
    }
}