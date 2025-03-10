using System.Collections.Generic;
using Nekki.Game.Context.Data.Spawn;
using UnityEngine;

namespace Nekki.Game.Entities.Objects.Data
{
    [CreateAssetMenu(order = -1, fileName = "ObjectSpawnConfig", menuName = "Nekki/Objects/ObjectSpawnConfig")]
    public class ObjectSpawnConfig : SpawnConfigBase
    {
        [Tooltip("Target objects for this config, they will spawn when this config conditions will true.")]
        public List<ObjectConfig> objects = new();
    }
}