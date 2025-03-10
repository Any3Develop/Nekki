using System;
using Nekki.Common.Events;
using Nekki.Common.Utilities;
using Nekki.Game.Context.Abstractions;
using Nekki.Game.Context.Events;
using Nekki.Game.Entities.Player.Abstractions;
using Nekki.Game.Entities.Units.Events;
using Nekki.Game.Scenarios.Data;
using Nekki.Game.Scenarios.Events;
using IServiceProvider = Nekki.Common.DependencyInjection.IServiceProvider;
using R3;

namespace Nekki.Game.Context
{
    public class GameContext : IGameContext
    {
        private int maxTime;
        
        public IPlayerViewModel Player { get; private set; }
        public IServiceProvider ServiceProvider { get; }
        public int Time { get; private set; }
        public int TimeLeft => maxTime - Time;
        public int UnitsTotalMax { get; private set; }
        public int UnitsSceneMax { get; private set; }
        public int UnitsAlive { get; private set; }
        public int UnitsSpawned  => UnitsAlive + UnitsDied;
        public int UnitsDied { get; private set; }
        public int UnitsLeft => UnitsTotalMax - UnitsSpawned;
        public bool Initialized { get; private set; }
        private const int TickValueMs = 1;
        private IDisposable subscribes;
        private bool isPaused;

        public GameContext(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
        
        public void Pause(bool value)
        {
            if (isPaused == value)
                return;

            isPaused = value;
        }

        public void Start(ScenarioConfig scenarioCfg, IPlayerViewModel player)
        {
            isPaused = false;
            maxTime = scenarioCfg.gameTime;
            
            UnitsTotalMax = scenarioCfg.maxUnitsTotal <= 0 ? int.MaxValue : scenarioCfg.maxUnitsTotal;
            UnitsSceneMax = scenarioCfg.maxUnitsScene <= 0 ? int.MaxValue : scenarioCfg.maxUnitsScene;
            subscribes?.Dispose();
            using var builder = new DisposableBuilder();
            
            builder.Add(Observable.Interval(TimeSpan.FromSeconds(TickValueMs))
                .Skip(TimeSpan.FromMilliseconds(scenarioCfg.delayedStartMs))
                .Subscribe(_ => Update()));
            
            builder.Add(MessageBroker.Receive<SenarioChangedEvent>()
                .Subscribe(evData => Pause(evData.Current.AnyFlags(ScenarioState.Paused))));
            
            builder.Add(MessageBroker.Receive<UnitSpawnedEvent>().Subscribe(_ =>
            {
                ++UnitsAlive;
                MessageBroker.Publish(new GameStatisticsChangedEvent());
            }));
            builder.Add(MessageBroker.Receive<UnitDiedEvent>().Subscribe(_ =>
            {
                --UnitsAlive;
                ++UnitsDied;
                MessageBroker.Publish(new GameStatisticsChangedEvent());
            }));

            subscribes = builder.Build();
            Player = player;
            Initialized = true;
        }

        private void Update()
        {
            if (isPaused)
                return;
            
            Time += TickValueMs;
            MessageBroker.Publish(new GameTimeChangedEvent());
            if (maxTime <= 0 || Time < maxTime)
                return;

            Time = maxTime;
            subscribes?.Dispose();
            subscribes = null;
            MessageBroker.Publish(new GameTimeEndedEvent());
        }

        public void End()
        {
            isPaused = false;
            subscribes?.Dispose();
            subscribes = null;
            Time = 0;
            Initialized = false;
        }
    }
}