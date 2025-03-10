using Nekki.Game.Stats.Data;

namespace Nekki.Game.Stats.Abstractions
{
    public interface IStatFactory
    {
        IRuntimeStat Create(StatData value);
    }
}