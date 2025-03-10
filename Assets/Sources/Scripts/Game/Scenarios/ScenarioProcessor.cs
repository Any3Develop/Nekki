using System;
using Nekki.Common.Events;
using Nekki.Common.LifecycleService;
using Nekki.Common.Utilities;
using Nekki.Game.Entities.Player.Data;
using Nekki.Game.Scenarios.Abstractions;
using Nekki.Game.Scenarios.Data;
using Nekki.Game.Scenarios.Events;
using Nekki.Lobby.Identity;
using R3;
using UnityEngine;

namespace Nekki.Game.Scenarios
{
    public class ScenarioProcessor : IScenarioProcessor, IDisposable, IInitable
    {
        private readonly IScenarioFactory scenarioFactory;
        private IDisposable subscribes;
        public IScenario Scenario { get; private set; }

        public ScenarioProcessor(IScenarioFactory scenarioFactory)
        {
            this.scenarioFactory = scenarioFactory;
        }

        public void Dispose()
        {
            subscribes?.Dispose();
            subscribes = null;
            Scenario?.Dispose();
            Scenario = null;
        }
        
        public void Initialize()
        {
            try
            {
                var levelData = UserIdentity.Redirections.GetArg<ScenarioData>();
                var playerData = UserIdentity.Redirections.GetArg<PlayerData>();
                using var builder = new DisposableBuilder();
            
                builder.Add(MessageBroker.Receive<SenarioChangedEvent>()
                    .Where(x => x.Scenario == Scenario)
                    .Subscribe(OnScenarioChanged));
            
                subscribes = builder.Build();
                Scenario = scenarioFactory.Create(levelData.Id, levelData, playerData);
                Scenario.Start();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        
        private void OnScenarioChanged(SenarioChangedEvent evData)
        {
            if (evData.Current.AnyFlags(ScenarioState.Ended | ScenarioState.None))
                Debug.Log($"Scenario : {evData.Scenario} ended!");
        }
    }
}