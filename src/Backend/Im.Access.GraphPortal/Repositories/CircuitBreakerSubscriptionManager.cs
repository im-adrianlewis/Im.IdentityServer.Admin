using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Im.Access.GraphPortal.Data;

namespace Im.Access.GraphPortal.Repositories
{
    public class CircuitBreakerPolicySubscriptionManager : IObservable<CircuitBreakerPolicyEntity>
    {
        private class Unsubscriber : IDisposable
        {
            private readonly List<IObserver<CircuitBreakerPolicyEntity>> _observers;
            private readonly IObserver<CircuitBreakerPolicyEntity> _observer;

            public Unsubscriber(
                List<IObserver<CircuitBreakerPolicyEntity>> observers,
                IObserver<CircuitBreakerPolicyEntity> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                {
                    _observers.Remove(_observer);
                }
            }
        }

        private readonly List<IObserver<CircuitBreakerPolicyEntity>> _observers =
            new List<IObserver<CircuitBreakerPolicyEntity>>();
        private readonly ICircuitBreakerPolicyStore _circuitBreakerPolicyStore;
        private readonly CancellationToken _cancellationToken;

        public CircuitBreakerPolicySubscriptionManager(
            ICircuitBreakerPolicyStore circuitBreakerPolicyStore,
            CancellationToken cancellationToken)
        {
            _circuitBreakerPolicyStore = circuitBreakerPolicyStore;
            _cancellationToken = cancellationToken;

            Task.Run(ChangeChecker);
        }

        private async Task ChangeChecker()
        {
            DateTimeOffset? lastChangesRead = null;
            while (!_cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var utcNow = DateTimeOffset.UtcNow;

                    IEnumerable<DbCircuitBreakerPolicy> entities;
                    if (lastChangesRead.HasValue)
                    {
                        entities = await _circuitBreakerPolicyStore
                            .GetCircuitBreakerPoliciesChangedSinceAsync(lastChangesRead.Value, _cancellationToken)
                            .ConfigureAwait(false);
                    }
                    else
                    {
                        entities = await _circuitBreakerPolicyStore
                            .GetCircuitBreakerPoliciesAsync(_cancellationToken)
                            .ConfigureAwait(false);
                    }

                    lastChangesRead = utcNow;

                    // Update all observers with changes
                    foreach (var entity in entities
                        .Select(entity =>
                            new CircuitBreakerPolicyEntity
                            {
                                Id = entity.Id,
                                Service = entity.ServiceName,
                                PolicyKey = entity.PolicyKey,
                                IsIsolated = entity.IsIsolated,
                                LastUpdated = entity.LastUpdated
                            }))
                    {
                        foreach (var observer in _observers)
                        {
                            observer.OnNext(entity);
                        }
                    }

                    await Task.Delay(1000, _cancellationToken).ConfigureAwait(false);
                }
                catch
                {
                }
            }
        }

        public IDisposable Subscribe(IObserver<CircuitBreakerPolicyEntity> observer)
        {
            _observers.Add(observer);
            return new Unsubscriber(_observers, observer);
        }
    }
}