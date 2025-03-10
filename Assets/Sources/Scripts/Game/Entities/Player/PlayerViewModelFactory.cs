using System.Linq;
using Nekki.Common.DependencyInjection;
using Nekki.Game.Abilities.Abstractions;
using Nekki.Game.Context.Abstractions;
using Nekki.Game.Entities.Player.Abstractions;
using Nekki.Game.Entities.Player.Data;
using Nekki.Game.Stats.Abstractions;
using Nekki.Game.Stats.Utils;
using Unity.Cinemachine;
using UnityEngine;

namespace Nekki.Game.Entities.Player
{
    public class PlayerViewModelFactory : IPlayerViewModelFactory
    {
        private readonly IAbilityFactory abilityFactory;
        private readonly IAbstractFactory abstractFactory;
        private readonly IServiceProvider serviceProvider;
        private readonly ISpawnPointProvider spawnPointProvider;

        public PlayerViewModelFactory(
            IAbilityFactory abilityFactory,
            IAbstractFactory abstractFactory, 
            IServiceProvider serviceProvider,
            ISpawnPointProvider spawnPointProvider)
        {
            this.abilityFactory = abilityFactory;
            this.abstractFactory = abstractFactory;
            this.serviceProvider = serviceProvider;
            this.spawnPointProvider = spawnPointProvider;
        }

        public IPlayerViewModel Create(PlayerData playerData)
        {
            var playerCfg = Resources.LoadAll<PlayerConfig>("Game/PlayerConfigs").First(x => x.id == playerData.Id);
            var commonCfg = Resources.Load<PlayerBaseConfig>("Game/PlayerConfigs/PlayerCfg_Base");

            var camera = Object.FindAnyObjectByType<CinemachineCamera>(); // TODO make it better, create camera instead
            var point = spawnPointProvider.Get(playerCfg.id.ToString(), 0, playerCfg.spawnIds, playerCfg.spawnFunction);
            var player = abstractFactory.CreateUnityObject<PlayerViewModel>(playerCfg.viewModelPrefab);
            var abilityCollection = serviceProvider.GetRequiredService<IAbilityCollection>();
            abilityCollection.AddRange(playerData.Abilities.Select(abilityFactory.Create));
            
            var statsCollection = serviceProvider
                .GetRequiredService<IStatsCollection>()
                .Merge(commonCfg.stats)
                .Merge(playerData.Stats);

            camera.Target.TrackingTarget = player.transform;
            player.Init(serviceProvider, statsCollection, abilityCollection, playerCfg, point.Position);
            return player;
        }
    }
}