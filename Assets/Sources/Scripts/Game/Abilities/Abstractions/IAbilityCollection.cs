using Nekki.Common.Collections;
using Nekki.Game.Abilities.Data;

namespace Nekki.Game.Abilities.Abstractions
{
    public interface IAbilityCollection : IRuntimeCollection<IAbility>
    {
        bool TryGet(AbilityId id, out IAbility result);
        IAbility Get(AbilityId id);
        bool Contains(AbilityId id);
    }
}