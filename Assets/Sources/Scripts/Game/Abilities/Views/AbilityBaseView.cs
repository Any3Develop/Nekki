using Nekki.Common.Pools;
using Nekki.Game.Abilities.Abstractions;

namespace Nekki.Game.Abilities.Views
{
    public abstract class AbilityBaseView : PoolableView, IAbilityView
    {
        public IAbility Ability { get; private set; }
        public void Init(IAbility ability) => Ability = ability;

        protected override void OnReleased()
        {
            base.OnReleased();
            Ability = null;
        }

        protected override void OnDisposed()
        {
            base.OnDisposed();
            Ability = null;
        }
    }
}