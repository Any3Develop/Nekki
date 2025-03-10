using Nekki.Common.Collections;
using Nekki.Game.Stats.Data;

namespace Nekki.Game.Stats.Abstractions
{
    public interface IStatsCollection : IRuntimeCollection<IRuntimeStat>
    {
        IRuntimeStat AddNew(StatData data);
        bool TryGet(StatType type, out IRuntimeStat result);
        IRuntimeStat Get(StatType type);
        bool Contains(StatType type);
    }
}