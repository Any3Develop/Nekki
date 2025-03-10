using Nekki.Game.Abilities.Data;

namespace Nekki.Game.Abilities.Abstractions
{
    public interface IAbilityFactory
    {
        IAbility Create(AbilityId id);
        IAbility Create(AbilityData data);
    }
}