using System.Collections.Generic;
using Nekki.Game.Common.Attributes;
using Nekki.Game.Common.Data;
using Nekki.Game.Stats.Data;
using Nekki.Game.Stats.Utils;
using UnityEngine;

namespace Nekki.Game.Entities.Units.Data
{
    [CreateAssetMenu(order = -1, fileName = "UnitConfig", menuName = "Nekki/Units/UnitConfig")]
    public class UnitConfig : ConfigBase
    {
        [Space,Header("Prototypes")]
        public UnitView viewPrefab;
        public UnitViewModel viewModelPrefab;
        
        [Space, Header("Behaviours")]
        public UnitClassType classType;
        public UnitBehaviorType behaviorType;
        public UnitMovementType movementType;
        [NavMeshAreaMask] public int walkableAreas;
        public UnitBehaviourSet behavioursSet;
        
        [Header("General")]
        public List<StatData> stats;

#if UNITY_EDITOR
        [ContextMenu(nameof(ResetStatsForInfantry))]
        private void ResetStatsForInfantry()
        {
            stats = new List<StatData>
            {
                new() {type = StatType.Level, value = 1},
                new() {type = StatType.Heath, value = 100},
                
                new() {type = StatType.MoveSpeed, value = 25},
                new() {type = StatType.MoveTurnSpeed, value = 250},
                new() {type = StatType.MoveAcceleration, value = 100},
                new() {type = StatType.Heght, value = 0.3f},
                new() {type = StatType.Radius, value = 2.5f},
                new() {type = StatType.Priority, value = 10}
            };
            
            OnValidate();
            UnityEditor.EditorUtility.SetDirty(this);
        }
        [ContextMenu(nameof(ResetStatsForAviation))]
        private void ResetStatsForAviation()
        {
            stats = new List<StatData>
            {
                new() {type = StatType.Level, value = 1},
                new() {type = StatType.Heath, value = 100},
                
                new() {type = StatType.MoveSpeed, value = 25},
                new() {type = StatType.MoveTurnSpeed, value = 250},
                new() {type = StatType.MoveAcceleration, value = 100},
                new() {type = StatType.FlyAltitude, value = 30},
                new() {type = StatType.FlyDumping, value = 1},
                new() {type = StatType.FlyTrunDumping, value = 1},
                
                new() {type = StatType.Heght, value = 0.3f},
                new() {type = StatType.Radius, value = 2.5f},
                new() {type = StatType.Priority, value = 20}
            };
            
            OnValidate();
            UnityEditor.EditorUtility.SetDirty(this);
        }
        
        protected override void Reset()
        {
            base.Reset();
            ResetStatsForInfantry();
            OnValidate();
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            stats.OnValidateStats();
        }
#endif
    }
}