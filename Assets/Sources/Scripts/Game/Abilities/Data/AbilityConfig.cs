using System.Collections.Generic;
using Nekki.Game.Abilities.Views;
using Nekki.Game.Stats.Data;
using Nekki.Game.Stats.Utils;
using UnityEngine;

namespace Nekki.Game.Abilities.Data
{
    // All unused fields are provided as an example to show the possibility of selecting and filtering game entities through a global system.
    [CreateAssetMenu(order = -1, fileName = "AbilityConfig", menuName = "Nekki/Abilities/AbilityConfig")]
    public class AbilityConfig : ScriptableObject
    {
        [Header("Prototypes")]
        public List<AbilityBaseView> viewPrefabs = new();

        [Header("General")] 
        [Tooltip("The unique identifier of the ability.")]
        public AbilityId id;
        [Tooltip("How the ability will be manipulated by the player and the game's environment.")]
        public AbilityType type;
        [Tooltip("How the ability will affect the chosen targets.")]
        public AbilityAffect affect;
        [Tooltip("Basic unique ability characteristics.")]
        public List<StatData> stats;

        [Header("Targets")] 
        [Tooltip("The number of targets is the minimum to trigger the ability. 0 disabled."), Min(0)]
        public int minTargets;
        [Tooltip("Number of maximum targets allowed for the ability. 0 disabled."), Min(0)]
        public int maxTargets;
        [Tooltip("A way of selecting targets.")]
        public AbilityTargeting targeting;
        [Tooltip("Determines the final number of targets.")]
        public TargetAoeFilter aoeFilter;
        [Tooltip("Filtering based on distance from the ability owner.")]
        public TargetDistanceFilter distanceFilter;
        [Tooltip("Filtering depending on the owner of the ability.")]
        public TargetOwnerFilter ownerFilter;
        
#if UNITY_EDITOR
        [ContextMenu(nameof(ResetStats))]
        private void ResetStats()
        {
            stats = new List<StatData>
            {
                new() {type = StatType.Range, value = 100},
                new() {type = StatType.Countdown, value = 0.25f},
                new() {type = StatType.Damage, value = 50},
                new() {type = StatType.MoveSpeed, value = 100},
            };
            
            OnValidate();
            UnityEditor.EditorUtility.SetDirty(this);
        }
        
        private void OnValidate()
        {
            stats.OnValidateStats();
        }
#endif
    }
}