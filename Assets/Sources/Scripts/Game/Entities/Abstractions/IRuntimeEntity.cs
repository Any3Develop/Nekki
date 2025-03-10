using Nekki.Game.Abilities.Abstractions;
using Nekki.Game.Stats.Abstractions;

namespace Nekki.Game.Entities.Abstractions
{
    public interface IRuntimeEntity
    {
        bool IsAlive { get; }
        IRuntimeEntityView View { get; }
        IStatsCollection StatsCollection { get; }
        IAbilityCollection AbilityCollection { get; }

        // TODO We can remove this and use some global system to handle incoming damage and stack effects.
        // TODO As a separate logic to relieve entities of this responsibility and lighten their code.
        bool ApplyDamage(float damage);
    }
}