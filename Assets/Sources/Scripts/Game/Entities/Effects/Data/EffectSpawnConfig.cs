using System.Collections.Generic;
using Nekki.Game.Context.Data.Spawn;
using UnityEngine;

namespace Nekki.Game.Entities.Effects.Data
{
    [CreateAssetMenu(order = -1, fileName = "EffectSpawnConfig", menuName = "Nekki/Effects/EffectSpawnConfig")]
    public class EffectSpawnConfig : SpawnConfigBase
    {
        [Tooltip("Target objects for this config, they will spawn when this config conditions will true.")]
        public List<EffectConfig> effects = new();
    }
}