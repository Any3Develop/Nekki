using Nekki.Common.DependencyInjection;
using Nekki.Common.Pools.Abstractions;
using Nekki.Game.Entities.Units.Abstractions;
using Nekki.Game.Entities.Units.Data;
using UnityEngine;

namespace Nekki.Game.Entities.Units
{
    public class UnitViewModelFactory : IUnitViewModelFactory
    {
        private int spawnIndex;
        private readonly IPool<IUnitViewModel> pool;
        private readonly IAbstractFactory abstractFactory;
        private readonly IServiceProvider serviceProvider;

        public UnitViewModelFactory(
            IPool<IUnitViewModel> pool, 
            IAbstractFactory abstractFactory,
            IServiceProvider serviceProvider)
        {
            this.pool = pool;
            this.abstractFactory = abstractFactory;
            this.serviceProvider = serviceProvider;
        }

        public IUnitViewModel Create(UnitConfig config, Vector3 position)
        {
            if (pool.TrySpawn(config.viewModelPrefab.GetType(), out UnitViewModel viewModel, false))
            {
                SetName(viewModel, config);
                viewModel.Init(config, position);
                viewModel.Spawn();
                return viewModel;
            }

            viewModel = abstractFactory.CreateUnityObject<UnitViewModel>(config.viewModelPrefab);
            viewModel.Construct(serviceProvider);
            viewModel.Init(config, position);
            SetName(viewModel, config);
            pool.Add(viewModel, true);
            return viewModel;
        }

        private static void SetName(UnitViewModel viewModel, UnitConfig config)
        {
            viewModel.name = $"{config.viewModelPrefab.name}_{config.viewPrefab.PoolableId}"; // TODO FOR TEST
        }
    }
}