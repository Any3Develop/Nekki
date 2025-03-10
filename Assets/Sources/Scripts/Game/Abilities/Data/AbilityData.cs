using System.Collections.Generic;
using Nekki.Game.Stats.Data;

namespace Nekki.Game.Abilities.Data
{
    public class AbilityData
    {
        public AbilityId Id { get; set; }
        public List<StatData> Stats { get; set; } = new();
    }
}