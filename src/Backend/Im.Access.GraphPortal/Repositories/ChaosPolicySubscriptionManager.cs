using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Im.Access.GraphPortal.Data;

namespace Im.Access.GraphPortal.Repositories
{
    public class ChaosPolicySubscriptionManager : IObservable<ChaosPolicyEntity>
    {
        private class Unsubscriber : IDisposable
        {
            private readonly List<IObserver<ChaosPolicyEntity>> _observers;
            private readonly IObserver<ChaosPolicyEntity> _observer;

            public Unsubscriber(
                List<IObserver<ChaosPolicyEntity>> observers,
                IObserver<ChaosPolicyEntity> observer)
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

        private readonly List<IObserver<ChaosPolicyEntity>> _observers =
            new List<IObserver<ChaosPolicyEntity>>();
        private readonly IChaosPolicyStore _chaosPolicyStore;
        private readonly CancellationToken _cancellationToken;

        public ChaosPolicySubscriptionManager(
            IChaosPolicyStore chaosPolicyStore,
            CancellationToken cancellationToken)
        {
            _chaosPolicyStore = chaosPolicyStore;
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

                    IEnumerable<DbChaosPolicy> entities;
                    if (lastChangesRead.HasValue)
                    {
                        entities = await _chaosPolicyStore
                            .GetChaosPoliciesChangedSinceAsync(lastChangesRead.Value, _cancellationToken)
                            .ConfigureAwait(false);
                    }
                    else
                    {
                        entities = await _chaosPolicyStore
                            .GetChaosPoliciesAsync(_cancellationToken)
                            .ConfigureAwait(false);
                    }

                    lastChangesRead = utcNow;

                    // Update all observers with changes
                    foreach (var entity in entities
                        .Select(entity =>
                            new ChaosPolicyEntity
                            {
                                Id = entity.Id,
                                Service = entity.ServiceName,
                                PolicyKey = entity.PolicyKey,
                                Enabled = entity.Enabled,
                                FaultEnabled = entity.FaultEnabled,
                                FaultInjectionRate = entity.FaultInjectionRate,
                                LatencyEnabled = entity.LatencyEnabled,
                                LatencyInjectionRate = entity.LatencyInjectionRate,
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

        public IDisposable Subscribe(IObserver<ChaosPolicyEntity> observer)
        {
            _observers.Add(observer);
            return new Unsubscriber(_observers, observer);
        }
    }
}