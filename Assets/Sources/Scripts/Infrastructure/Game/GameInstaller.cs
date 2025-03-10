using Nekki.Common.Pools;
using Nekki.Game.Abilities;
using Nekki.Game.Abilities.Abstractions;
using Nekki.Game.Context;
using Nekki.Game.Context.Spawn;
using Nekki.Game.Entities.Player;
using Nekki.Game.Entities.Units;
using Nekki.Game.Entities.Units.Abstractions;
using Nekki.Game.Scenarios;
using Nekki.Game.Stats;
using Nekki.Game.Stats.Abstractions;
using Nekki.Game.UI;
using Nekki.Game.UI.AbilityList;
using Nekki.Game.UI.GameStatistics;
using Nekki.Infrastructure.Common;
using Zenject;

namespace Nekki.Infrastructure.Game
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            ProjectContextInstaller.InstallSettings();
            InstallUI();
            InstallFactories();
            InstallPools();
            InstallContext();
            InstallEntryPoint();
        }

        private void InstallFactories()
        {
            Container
                .BindInterfacesTo<ScenarioFactory>()
                .AsSingle()
                .NonLazy();

            Container
                .BindInterfacesTo<AbilityFactory>()
                .AsSingle()
                .NonLazy();

            Container
                .BindInterfacesTo<AbilityViewFactory>()
                .AsSingle();

            Container
                .BindInterfacesTo<UnitViewFactory>()
                .AsSingle();

            Container
                .BindInterfacesTo<UnitViewModelFactory>()
                .AsSingle();

            Container
                .BindInterfacesTo<UnitMovementFactory>()
                .AsSingle();

            Container
                .BindInterfacesTo<StatFactory>()
                .AsSingle();

            Container
                .BindInterfacesTo<PlayerViewModelFactory>()
                .AsSingle();

            Container
                .BindInterfacesTo<PlayerViewFactory>()
                .AsTransient();

            Container
                .BindInterfacesTo<PlayerMovementFactory>()
                .AsTransient();

            Container
                .BindInterfacesTo<PlayerAnimatorFactory>()
                .AsTransient();
        }

        private void InstallPools()
        {
            Container
                .BindInterfacesTo<RuntimePool<IUnitView>>()
                .AsSingle();
            
            Container
                .BindInterfacesTo<RuntimePool<IUnitViewModel>>()
                .AsSingle();
            
            Container
                .BindInterfacesTo<RuntimePool<IRuntimeStat>>()
                .AsSingle();
            
            Container
                .BindInterfacesTo<RuntimePool<IAbilityView>>()
                .AsSingle();
        }

        private void InstallContext()
        {
            Container
                .BindInterfacesTo<StatsCollection>()
                .AsTransient();
            
            Container
                .BindInterfacesTo<AbilityCollection>()
                .AsTransient();
            
            Container
                .BindInterfacesTo<PositionProvider>()
                .AsSingle();
            
            Container
                .BindInterfacesTo<SpawnPointProvider>()
                .AsSingle();
            
            Container
                .BindInterfacesTo<GameContext>()
                .AsSingle();

        }
        
        private void InstallUI()
        {
            Container
                .BindInterfacesTo<SetupGameUIGroup>()
                .AsSingle()
                .NonLazy();
            
            Container
                .BindInterfacesTo<GameStatistics>()
                .AsSingle()
                .NonLazy();
            
            Container
                .BindInterfacesTo<AbilityListViewModel>()
                .AsSingle()
                .NonLazy();
        }
        
        private void InstallEntryPoint()
        {
            Container
                .BindInterfacesTo<ScenarioProcessor>()
                .AsSingle()
                .NonLazy();
        }
    }
}