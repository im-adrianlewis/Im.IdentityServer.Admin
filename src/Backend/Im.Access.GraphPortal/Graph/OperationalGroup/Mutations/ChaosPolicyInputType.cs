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
            Field(p => p.Enabled);
            Field(p => p.FaultEnabled);
            Field(p => p.FaultInjectionRate);
            Field(p => p.LatencyEnabled);
            Field(p => p.LatencyInjectionRate);
        }
    }
}