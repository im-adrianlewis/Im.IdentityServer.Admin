using GraphQL.Types;
using Im.Access.GraphPortal.Repositories;

namespace Im.Access.GraphPortal.Graph.OperationalGroup
{
    public class ChaosPolicyType : ObjectGraphType<ChaosPolicyEntity>
    {
        public ChaosPolicyType()
        {
            Name = "ChaosPolicy";
            Description = "Describes the state of a service chaos policy";

            Field(e => e.Service).Description("Service this policy applies to.");
            Field(e => e.PolicyKey).Description("Name of this chaos policy.");
            Field(e => e.Enabled).Description("Current state of the circuit breaker.");
            Field(e => e.FaultEnabled).Description("Current state of the circuit breaker.");
            Field(e => e.FaultInjectionRate).Description("Current state of the circuit breaker.");
            Field(e => e.LatencyEnabled).Description("Current state of the circuit breaker.");
            Field(e => e.LatencyInjectionRate).Description("Current state of the circuit breaker.");
            Field(e => e.LastUpdated).Description("When the state last changed.");
        }
    }
}