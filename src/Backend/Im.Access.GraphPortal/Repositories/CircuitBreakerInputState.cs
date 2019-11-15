using System.ComponentModel;

namespace Im.Access.GraphPortal.Repositories
{
    public enum CircuitBreakerInputState
    {
        [Description("Manually isolate the circuit breaker disconnecting source from target.")]
        Isolate,

        [Description("Reset the circuit breaker so it is back to being closed and connected.")]
        Reset
    }
}