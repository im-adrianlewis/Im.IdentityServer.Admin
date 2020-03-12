using System;

namespace Im.Access.GraphPortal.Repositories
{
    public class ChaosPolicyEntity
    {
        public Guid Id { get; set; }

        public string PolicyKey { get; set; }

        public string Service { get; set; }

        public bool Enabled { get; set; }

        public bool FaultEnabled { get; set; }

        public double FaultInjectionRate { get; set; }

        public bool LatencyEnabled { get; set; }

        public double LatencyInjectionRate { get; set; }

        public DateTimeOffset LastUpdated { get; set; }
    }
}