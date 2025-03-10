using System.Collections.Generic;
using Nekki.Game.Stats.Data;
using Nekki.Game.Stats.Utils;
using UnityEngine;

namespace Nekki.Game.Entities.Player.Data
{
    [CreateAssetMenu(order = -1, fileName = "CommonPlayerConfig", menuName = "Nekki/Player/CommonPlayerConfig")]
    public class PlayerBaseConfig : ScriptableObject
    {
        [Space, Header("General")]
        public List<StatData> stats;

#if UNITY_EDITOR
        [ContextMenu(nameof(ResetStats))]
        private void ResetStats()
        {
            stats = new List<StatData>
            {
                new() {type = StatType.Level, value = 1, useMin = true, min = 1},
                new() {type = StatType.Heath, value = 100, useMin = true, min = 0},
                new() {type = StatType.MoveSpeed, value = 25},
                new() {type = StatType.MoveTurnSpeed, value = 50},
                new() {type = StatType.MoveAcceleration, value = 100},
                new() {type = StatType.MoveDumpingFactor, value = 9.5f},
                new() {type = StatType.Heght, value = 0.4f},
                new() {type = StatType.Radius, value = 2.5f},
            };
            
            OnValidate();
            UnityEditor.EditorUtility.SetDirty(this);
        }
        
        protected void Reset()
        {
            ResetStats();
        }

        protected void OnValidate()
        {
            stats.OnValidateStats();
        }
#endif
    }
}