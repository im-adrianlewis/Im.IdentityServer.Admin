using System;

namespace Im.Access.GraphPortal.Data
{
    public class DbCircuitBreakerPolicy
    {
        public Guid Id { get; set; }

        public string MachineName { get; set; }

        public string ServiceName { get; set; }

        public string PolicyKey { get; set; }

        public bool IsIsolated { get; set; }

        public DateTimeOffset LastUpdated { get; set; }
    }
}