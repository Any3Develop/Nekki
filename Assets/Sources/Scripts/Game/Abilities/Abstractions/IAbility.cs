using Nekki.Game.Abilities.Data;
using Nekki.Game.Entities.Abstractions;
using Nekki.Game.Stats.Abstractions;

namespace Nekki.Game.Abilities.Abstractions
{
    public interface IAbility
    {
        bool Enabled { get; }
        AbilityConfig Config { get; }
        IRuntimeEntity Owner { get; }
        IStatsCollection StatsCollection { get; }

        void ApplyOwnership(IRuntimeEntity value);
        void Enable(bool value);
        bool CanExecute();
        void Execute();
    }
}