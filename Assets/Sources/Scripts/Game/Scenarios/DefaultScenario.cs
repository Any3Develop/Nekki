using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Nekki.Game.Context.Abstractions;
using Nekki.Game.Context.Data.Spawn;
using Nekki.Game.Entities.Effects.Data;
using Nekki.Game.Entities.Objects.Data;
using Nekki.Game.Entities.Player.Abstractions;
using Nekki.Game.Entities.Player.Data;
using Nekki.Game.Entities.Units.Abstractions;
using Nekki.Game.Entities.Units.Data;
using Nekki.Game.Scenarios.Data;
using Nekki.Game.Stats.Data;
using UnityEngine;

namespace Nekki.Game.Scenarios
{
    [Scenario(ScenarioId.Default2D)]
    [Scenario(ScenarioId.Default3D)]
    public class DefaultScenario : ScenarioBase
    {
        private readonly IGameContext context;
        private readonly IUnitViewModelFactory unitViewModelFactory;
        private readonly IPlayerViewModelFactory playerViewModelFactory;
        private readonly ISpawnPointProvider spawnPointProvider;
        private readonly IPositionProvider positionProvider;
        private readonly ScenarioData scenarioData;
        private readonly PlayerData playerData;
        private ScenarioConfig scenarioCfg;

        private readonly CancellationTokenSource lifetime;
        private readonly CancellationToken token;

        public DefaultScenario(
            IGameContext context,
            IUnitViewModelFactory unitViewModelFactory,
            IPlayerViewModelFactory playerViewModelFactory,
            ISpawnPointProvider spawnPointProvider,
            IPositionProvider positionProvider,
            ScenarioData scenarioData,
            PlayerData playerData)
        {
            this.context = context;
            this.unitViewModelFactory = unitViewModelFactory;
            this.playerViewModelFactory = playerViewModelFactory;
            this.spawnPointProvider = spawnPointProvider;
            this.positionProvider = positionProvider;
            this.scenarioData = scenarioData;
            this.playerData = playerData;
            lifetime = new CancellationTokenSource();
            token = lifetime.Token;
        }

        protected override void OnStarted()
        {
            scenarioCfg = Resources.LoadAll<ScenarioConfig>("Game/ScenarioConfigs")
                .FirstOrDefault(x => x.id == scenarioData.Id && x.level == scenarioData.Level);
            
            spawnPointProvider.Start(scenarioCfg);
            context.Start(scenarioCfg, playerViewModelFactory.Create(playerData));
            StartAsync().Forget();
        }

        protected override void OnEnded()
        {
            spawnPointProvider?.End();
            context?.End();
            if (!lifetime.IsCancellationRequested)
            {
                lifetime?.Cancel();
                lifetime?.Dispose();
            }
        }

        protected override void OnDisposed()
        {
            if (!lifetime.IsCancellationRequested)
            {
                lifetime?.Cancel();
                lifetime?.Dispose();
            }
        }

        protected virtual async UniTask StartAsync()
        {
            try
            {
                var spawnConfigs = scenarioCfg.unitsScenario
                    .Concat<SpawnConfigBase>(scenarioCfg.objectsScenario)
                    .Concat(scenarioCfg.effectsScenario);
                
                foreach (var cfg in spawnConfigs)
                    SetDefault(cfg);
                
                await UniTask.Delay(scenarioCfg.delayedStartMs, cancellationToken: token);

                ExecuteLoopAsync().Forget();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                End();
            }
        }

        private async UniTask ExecuteLoopAsync()
        {
            try
            {
                var hasUnitsCfg = scenarioCfg.unitsScenario.Length > 0;
                var hasObjectsCfg = scenarioCfg.objectsScenario.Length > 0;
                var hasEffectsCfg = scenarioCfg.effectsScenario.Length > 0;
                var levelStat = context.Player.StatsCollection.Get(StatType.Level);
                var hpStat = context.Player.StatsCollection.Get(StatType.Heath);
                var tickDelay = TimeSpan.FromMilliseconds(1000);
                
                while (Application.isPlaying && !token.IsCancellationRequested)
                {
                    var diedCount = context.UnitsDied;
                    var progress = (diedCount / context.UnitsTotalMax) * 100; // TODO Improve the game progress
                    var level = (int)levelStat.Current;
                    var time = context.Time;

                    if (hasUnitsCfg)
                        ExecuteScenario(scenarioCfg.unitsScenario, level, diedCount, progress, time, SpawnUnits);

                    if (hasObjectsCfg)
                        ExecuteScenario(scenarioCfg.objectsScenario, level, diedCount, progress, time, SpawnObjects);

                    if (hasEffectsCfg)
                        ExecuteScenario(scenarioCfg.effectsScenario, level, diedCount, progress, time, SpawnEffects);
                    
                    await UniTask.Delay(tickDelay, cancellationToken: token);

                    if (!CheckScenarioEnded(hpStat.Current)) 
                        continue;
                    
                    End();
                    break;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                End();
            }
        }

        private void ExecuteScenario<T>(
            IEnumerable<T> configs,
            int level,
            int diedCount,
            int progress,
            int time,
            Func<T, bool> execute) where T : SpawnConfigBase
        {
            foreach (var cfg in configs.Where(cfg => ProcessConditions(cfg, level, diedCount, progress, time) && execute(cfg)))
                PassConditions(cfg, level, diedCount, progress, time);
        }
        
        private bool SpawnUnits(UnitSpawnConfig cfg)
        {
            var available = context.UnitsSceneMax - context.UnitsAlive;
            if (available <= 0)
                return false;
            
            available = Math.Min(available, cfg.units.Count);
            for (var index = 0; index < available; index++)
            {
                var unitCfg = positionProvider.Apply(cfg.id + 1, index, cfg.units, cfg.listOrderFunction);
                var point = spawnPointProvider.Get(cfg.id, index, cfg.spawnIds, cfg.spawnFunction);
                unitViewModelFactory.Create(unitCfg, point.Position);
            }
            return true;
        }

        private bool SpawnObjects(ObjectSpawnConfig cfg) => true; // TODO implement 
        private bool SpawnEffects(EffectSpawnConfig cfg) => true; // TODO implement 

        protected virtual bool CheckScenarioEnded(float playerHealth) // TODO implement more specific conditions
            => playerHealth <= 0 || (context.UnitsLeft <= 0 && context.UnitsAlive <= 0);

        private static bool ProcessConditions(
            SpawnConfigBase cfg,
            int level,
            int diedCount,
            int progress,
            int time)
        {
            return (!cfg.whenUnitsDied.enabled || cfg.whenUnitsDied.IsConditionTrue(diedCount))
                   && (!cfg.whenGameProgress.enabled || cfg.whenGameProgress.IsConditionTrue(progress))
                   && (!cfg.whenPlayerLevel.enabled || cfg.whenPlayerLevel.IsConditionTrue(level))
                   && (!cfg.whenGameTime.enabled || cfg.whenGameTime.IsConditionTrue(time));
        }

        private static void PassConditions(
            SpawnConfigBase cfg,
            int level,
            int diedCount,
            int progress,
            int time)
        {
            cfg.whenUnitsDied.Passed(diedCount);
            cfg.whenGameProgress.Passed(progress);
            cfg.whenPlayerLevel.Passed(level);
            cfg.whenGameTime.Passed(time);
        }

        private static void SetDefault(SpawnConfigBase cfg)
        {
            cfg.whenUnitsDied.Default();
            cfg.whenGameProgress.Default();
            cfg.whenPlayerLevel.Default();
            cfg.whenGameTime.Default();
        }
    }
}