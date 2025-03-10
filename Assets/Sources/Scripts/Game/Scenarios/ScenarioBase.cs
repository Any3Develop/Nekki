using System;
using System.Collections.Generic;
using System.Linq;
using Nekki.Common.Events;
using Nekki.Common.Utilities;
using Nekki.Game.Scenarios.Abstractions;
using Nekki.Game.Scenarios.Data;
using Nekki.Game.Scenarios.Events;
using R3;

namespace Nekki.Game.Scenarios
{
    public abstract class ScenarioBase : IScenario
    {
        protected readonly ReactiveProperty<ScenarioState> StateInternal = new(ScenarioState.None);
        protected IDisposable subscriptions;
        
        public ScenarioState State => StateInternal.CurrentValue;
        public virtual IEnumerable<IScenario> Nested { get; } = Enumerable.Empty<IScenario>();

        public void Start()
        {
            if (!State.AnyFlags(ScenarioState.None))
                return;

            using var builder = new DisposableBuilder();
            builder.Add(StateInternal.Pairwise()
                .Select(x => new SenarioChangedEvent(x.Current, x.Previous, this))
                .Subscribe(MessageBroker.Publish));
            builder.Add(StateInternal);
            subscriptions = builder.Build();
            
            OnStarted();
            StateInternal.Value = ScenarioState.Playing;
        }

        public void Pause(bool value)
        {
            if (State.AnyFlags(ScenarioState.None | ScenarioState.Ended)
                || (value && !State.AnyFlags(ScenarioState.Paused))
                || (!value && State.AnyFlags(ScenarioState.Paused)))
                return;

            if (value)
            {
                State.AddFlag(ScenarioState.Paused);
                OnPaused();
                return;
            }

            State.AddFlag(ScenarioState.Paused);
            OnResume();
        }

        public void End()
        {
            if (State.AnyFlags(ScenarioState.None | ScenarioState.Ended))
                return;

            Pause(false);
            StateInternal.Value = State.AddFlag(ScenarioState.Ended);
            OnEnded();
            
            subscriptions?.Dispose();
            subscriptions = null;
        }

        public void Dispose()
        {
            if (State.AllFlags(ScenarioState.None))
                return;

            subscriptions?.Dispose();
            subscriptions = null;
            OnDisposed();
        }

        protected virtual void OnStarted() {}
        protected virtual void OnPaused() {}
        protected virtual void OnResume() {}
        protected virtual void OnEnded() {}
        protected virtual void OnDisposed() {}
    }
}