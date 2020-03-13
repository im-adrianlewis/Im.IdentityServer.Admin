using GraphQL.Types;
using Im.Access.GraphPortal.Repositories;

namespace Im.Access.GraphPortal.Graph.OperationalGroup.Mutations
{
    public class ChaosPolicyInputType : InputObjectGraphType<ChaosPolicyInput>
    {
        public ChaosPolicyInputType()
        {
            Name = "ChaosPolicyInput";

            Field(p => p.ServiceName);
            Field(p => p.PolicyKey);
            Field(p => p.Enabled, true);
            Field(p => p.FaultEnabled, true);
            Field(p => p.FaultInjectionRate, true);
            Field(p => p.LatencyEnabled, true);
            Field(p => p.LatencyInjectionRate, true);
        }
    }
}