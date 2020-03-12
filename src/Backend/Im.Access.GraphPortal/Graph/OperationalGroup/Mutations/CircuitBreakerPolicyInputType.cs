using GraphQL.Types;

namespace Im.Access.GraphPortal.Graph.OperationalGroup.Mutations
{
    public class CircuitBreakerPolicyInputType : InputObjectGraphType
    {
        public CircuitBreakerPolicyInputType()
        {
            Name = "CircuitBreakerPolicyInput";

            Field<NonNullGraphType<StringGraphType>>("serviceName");
            Field<NonNullGraphType<StringGraphType>>("policyKey");
            Field<NonNullGraphType<BooleanGraphType>>("isIsolated");
        }
    }

    public class ChaosPolicyInputType : InputObjectGraphType
    {
        public ChaosPolicyInputType()
        {
            Name = "ChaosPolicyInput";

            Field<NonNullGraphType<StringGraphType>>("serviceName");
            Field<NonNullGraphType<StringGraphType>>("policyKey");
            Field<BooleanGraphType>("enabled");
            Field<BooleanGraphType>("faultEnabled");
            Field<FloatGraphType>("faultInjectionRate");
            Field<BooleanGraphType>("latencyEnabled");
            Field<FloatGraphType>("latencyInjectionRate");
        }
    }
}