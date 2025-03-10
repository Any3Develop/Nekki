using Nekki.Common.Pools.Abstractions;

namespace Nekki.Game.Abilities.Abstractions
{
    public interface IAbilityView : ISpwanPoolable
    {
        IAbility Ability { get; }
    }
}