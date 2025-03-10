using System.Collections.Generic;
using Nekki.Game.Context.Data.Spawn;
using UnityEngine;

namespace Nekki.Game.Entities.Units.Data
{
    [CreateAssetMenu(order = -1, fileName = "UnitSpawnConfig", menuName = "Nekki/Units/UnitSpawnConfig")]
    public class UnitSpawnConfig : SpawnConfigBase
    {
        [Tooltip("Provide how to select from the list a next unit.")]
        public FunctionSelector listOrderFunction;
        
        [Tooltip("Target units for this config, they will spawn when this config conditions will true.")]
        public List<UnitConfig> units = new();
    }
}