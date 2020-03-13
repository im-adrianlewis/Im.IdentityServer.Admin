namespace Im.Access.GraphPortal.Repositories
{
    public class CircuitBreakerPolicyInput
    {
        public string ServiceName { get; set; }

        public string PolicyKey { get; set; }

        public bool IsIsolated { get; set; }
    }

    public class ChaosPolicyInput
    {
        public string ServiceName { get; set; }

        public string PolicyKey { get; set; }

        public bool? Enabled { get; set; }

        public bool? FaultEnabled { get; set; }

        public double? FaultInjectionRate { get; set; }

        public bool? LatencyEnabled { get; set; }

        public double? LatencyInjectionRate { get; set; }
    }
}