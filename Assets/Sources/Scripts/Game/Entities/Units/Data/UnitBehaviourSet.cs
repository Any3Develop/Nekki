using System;
using Nekki.Common.SerializableDictionary;
using Unity.Behavior;
using UnityEngine;

namespace Nekki.Game.Entities.Units.Data
{
    [CreateAssetMenu(order = -1, fileName = "UnitBehaviourSet", menuName = "Nekki/Units/UnitBehaviourSet")]
    public class UnitBehaviourSet : ScriptableObject
    {
        [Serializable]
        private class BehaviorsMap : SerializableDictionary<UnitBehaviorType, BehaviorGraph> {}

        [SerializeField] private BehaviorsMap behavioursSet = new();

        public BehaviorGraph Get(UnitBehaviorType value) => behavioursSet[value];
    }
}