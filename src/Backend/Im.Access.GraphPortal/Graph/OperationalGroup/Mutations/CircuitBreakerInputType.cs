using GraphQL.Types;

namespace Im.Access.GraphPortal.Graph.OperationalGroup.Mutations
{
    public class CircuitBreakerInputType : InputObjectGraphType
    {
        public CircuitBreakerInputType()
        {
            Name = "CircuitBreakerInput";

            Field<NonNullGraphType<StringGraphType>>("name");
            Field<NonNullGraphType<CircuitBreakerInputStateEnum>>("state");
        }
    }
}