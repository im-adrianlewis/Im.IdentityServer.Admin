using GraphQL.Types;
using Im.Access.GraphPortal.Repositories;

namespace Im.Access.GraphPortal.Graph.OperationalGroup.Mutations
{
    public class CircuitBreakerPolicyInputType : InputObjectGraphType<CircuitBreakerPolicyInput>
    {
        public CircuitBreakerPolicyInputType()
        {
            Name = "CircuitBreakerPolicyInput";

            Field(p => p.ServiceName);
            Field(p => p.PolicyKey);
            Field(p => p.IsIsolated);
        }
    }
}