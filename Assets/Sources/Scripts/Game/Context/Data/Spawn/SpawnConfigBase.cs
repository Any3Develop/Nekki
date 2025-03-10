using System.Collections.Generic;
using Nekki.Game.Common.Data;
using UnityEngine;

namespace Nekki.Game.Context.Data.Spawn
{
    public abstract class SpawnConfigBase : ConfigBase
    {
        [Space, Header("Conditions")]
        [Tooltip("Starts : When units with this count died, then will start to spawn this config.\n\n" +
                 "Ends : When units with this count died, then will end to spawn this config.\n\n" +
                 "Each : When each time units with this count died, then will spawn this config.")]
        public SpawnCondition whenUnitsDied;

        [Tooltip("Starts : When game time reached this value in seconds, then will start to spawn this config.\n\n" +
                 "Ends : When game time reached this value in seconds, then will end to spawn this config.\n\n" +
                 "Each : When game time each times reached this value in seconds, then will spawn this config.")]
        public SpawnCondition whenGameTime;

        [Tooltip(
            "Starts : When game reached the progress in Percents greater or equal this value, then will start to spawn this config.\n\n" +
            "Ends : When game reached the progress in Percents greater or equal this value, then will end to spawn this config.\n\n" +
            "Each : When each time game reached the progress in Percents equal this value, then will spawn this config.")]
        public SpawnCondition whenGameProgress;

        [Tooltip(
            "Starts : When the player reached the level greater or equal this value will start to spawn this config.\n\n" +
            "Ends : When the player reached the level greater or equal this value, then will end to spawn this config.\n\n" +
            "Each : When each time the player reached the level equal this value will spawn this config.")]
        public SpawnCondition whenPlayerLevel;
        
        [Space, Header("Spawn")]
        [Tooltip("Provide how to select from the list of PositionIds a next spawn id.")]
        public FunctionSelector spawnFunction;

        [Tooltip("At what points can units of this config spawn?")]
        public List<SpawnId> spawnIds = new();
        
        
#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            whenUnitsDied.Reset(-1);
            whenGameTime.Reset(-1);
            whenGameProgress.Reset(-1);
            whenGameProgress.Reset(-1);
            whenPlayerLevel.Reset(-1);
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            whenUnitsDied.ClampMax(-1);
            whenGameTime.ClampMax(-1);
            whenGameProgress.Clamp(-1, 100);
            whenGameProgress.ClampMax(-1);
            whenPlayerLevel.ClampMax(-1);
        }
#endif
    }
}