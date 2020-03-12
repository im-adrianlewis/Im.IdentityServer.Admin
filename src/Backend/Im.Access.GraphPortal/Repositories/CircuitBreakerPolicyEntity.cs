using System;

namespace Im.Access.GraphPortal.Repositories
{
    public class CircuitBreakerPolicyEntity
    {
        public Guid Id { get; set; }

        public string PolicyKey { get; set; }

        public string Service { get; set; }

        public bool IsIsolated { get; set; }

        public DateTimeOffset LastUpdated { get; set; }
    }
}