using System;
using Nekki.Common.Pools.Abstractions;
using Nekki.Game.Stats.Data;

namespace Nekki.Game.Stats.Abstractions
{
    public interface IRuntimeStat : IPoolable
    {
        event Action<IRuntimeStat> OnChanged;
        
        float Current { get; }
        float Min { get; }
        float Max { get; }
        StatType Type { get; }
        
        void Set(float value);
        void SetMax(float value);
        void SetMin(float value);
        void Add(float value);
        void Subtract(float value);
        void SetToMax();
    }
}