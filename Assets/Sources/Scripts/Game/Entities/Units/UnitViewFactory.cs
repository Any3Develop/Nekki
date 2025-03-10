using Nekki.Common.DependencyInjection;
using Nekki.Common.Pools.Abstractions;
using Nekki.Game.Entities.Units.Abstractions;

namespace Nekki.Game.Entities.Units
{
    public class UnitViewFactory : IUnitViewFactory
    {
        private readonly IPool<IUnitView> pool;
        private readonly IAbstractFactory abstractFactory;

        public UnitViewFactory(
            IPool<IUnitView> pool,
            IAbstractFactory abstractFactory)
        {
            this.pool = pool;
            this.abstractFactory = abstractFactory;
        }

        public IUnitView Create(IUnitViewModel value)
        {
            var unitId = value.Config.viewPrefab.PoolableId;
            if (pool.TrySpawn(result => result.PoolableId == unitId, out UnitView view, false))
            {
                SetName(view, value);
                view.Init(value);
                view.Spawn();
                return view;
            }

            view = abstractFactory.CreateUnityObject<UnitView>(value.Config.viewPrefab);
            view.Init(value);
            SetName(view, value);
            pool.Add(view, true);
            return view;
        }
        
        private void SetName(UnitView view, IUnitViewModel viewModel)
        {
            view.name = $"{viewModel.Config.viewPrefab.name}_{view.PoolableId}"; // TODO FOR TEST
        }
    }
}