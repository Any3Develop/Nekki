using System;
using Nekki.Common.DependencyInjection;
using Nekki.Common.Pools.Abstractions;
using Nekki.Game.Abilities.Abstractions;
using Nekki.Game.Abilities.Views;

namespace Nekki.Game.Abilities
{
    public class AbilityViewFactory : IAbilityViewFactory
    {
        private readonly IAbstractFactory abstractFactory;
        private readonly IPool<IAbilityView> abilityViewPool;

        public AbilityViewFactory(IAbstractFactory abstractFactory, IPool<IAbilityView> abilityViewPool)
        {
            this.abstractFactory = abstractFactory;
            this.abilityViewPool = abilityViewPool;
        }

        public IAbilityView Create(IAbility ability, int index = 0)
        {
            if (index < 0 || index >= ability.Config.viewPrefabs.Count)
                throw new IndexOutOfRangeException($"Provide correct index to create : {nameof(IAbilityView)}, incoming index: {index}, available indices from 0 up to : {ability.Config.viewPrefabs.Count-1}");
           
            var prefab = ability.Config.viewPrefabs[index];
            if (abilityViewPool.TrySpawn(result => result.PoolableId == prefab.PoolableId, out AbilityBaseView view, false))
            {
                view.Init(ability);
                view.Spawn();
                return view;
            }

            view = abstractFactory.CreateUnityObject<AbilityBaseView>(prefab);
            abilityViewPool.Add(view, true, false);

            view.Init(ability);
            view.Spawn();
            return view;
        }

        public TView Create<TView>(IAbility ability, int index = 0) where TView : IAbilityView => (TView)Create(ability, index);
    }
}