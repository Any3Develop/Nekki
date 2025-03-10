using System;
using Nekki.Game.Stats.Abstractions;
using Nekki.Game.Stats.Data;

namespace Nekki.Game.Stats
{
    public class RuntimeStat : IRuntimeStat
    {
        private StatData stat;
        private ModifiableFloat current = new();
        private ModifiableFloat min = new();
        private ModifiableFloat max = new();
        
        public string PoolableId { get; } = Guid.NewGuid().ToString();
        public event Action<IRuntimeStat> OnChanged;
        public StatType Type => stat.type;
        public float Current => current;
        public float Min => min;
        public float Max => max;

        public void Init(StatData value)
        {
            stat = value;
            current.Override(CalmpMinMax(stat.value));
            min.Override(stat.min);
            max.Override(stat.max);
        }

        public void Set(float value)
        {
            var newModifier = CalmpMinMax(value - current.Value);
            if (Math.Abs(newModifier - current.Modifier) < 0.01f)
                return;
            
            current.Set(newModifier);
            OnChanged?.Invoke(this);
        }

        public void SetMax(float value)
        {
            min.Override(value);
        }

        public void SetMin(float value)
        {
            max.Override(value);
        }

        public void Add(float value)
        {
            var virtualTotal = CalmpMinMax(current.Total + value); // clamp a new virtual total
            var newValue = virtualTotal - current.Total; // find how much we will add
            if (newValue == 0) // if any changes won't come
                return;
            
            current.Add(newValue); // a new modifier
            OnChanged?.Invoke(this);
        }

        public void Subtract(float value) => Add(-value);

        public void SetToMax()
        {
            if (current.Modifier == 0)
                return;
            
            current.Set(0);
            OnChanged?.Invoke(this);
        }

        public void Release()
        {
            OnChanged = null;
            current?.Set(0);
            min?.Set(0);
            max?.Set(0);
        }
        
        public void Dispose()
        {
            OnChanged = null;
            current = null;
            min = null;
            max = null;
        }

        private float CalmpMinMax(float curr) 
            => Math.Clamp(curr, stat.useMin ? min : float.MinValue, stat.useMax ? max : float.MaxValue);
    }
}