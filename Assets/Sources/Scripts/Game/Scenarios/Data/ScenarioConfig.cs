using Nekki.Game.Entities.Effects.Data;
using Nekki.Game.Entities.Objects.Data;
using Nekki.Game.Entities.Units.Data;
using UnityEngine;

namespace Nekki.Game.Scenarios.Data
{
    [CreateAssetMenu(order = -1, fileName = "ScenarioConfig", menuName = "Nekki/Scenarios/ScenarioConfig")]
    public class ScenarioConfig : ScriptableObject
    {
        [Header("General")]
        [Tooltip("Which scenario code will execute with this config.")]
        public ScenarioId id;
        
        [Tooltip("Level as uniq id of the scenario config, to chose from many of configs."), Min(0)]
        public int level;
        
        [Tooltip("How many time in seconds the game will playing. -1 is infinity."), Min(-1)]
        public int gameTime = -1;
        
        [Tooltip("The maximum limit of unit spawns in a scenario at all times. 0 is Infinity."), Min(0)]
        public int maxUnitsTotal;
        
        [Tooltip("The maximum limit of unit spawns in a scenario at the moment. 0 is Infinity."), Min(0)]
        public int maxUnitsScene;
        
        [Tooltip("A delay in Milliseconds before the game starts, when everything has loaded and the player has gained control over the character."), Min(0)]
        public int delayedStartMs = 3_000;

        [Space, Header("Providers")]
        [Tooltip("Available units and their conditions of occurrence for this scenario.")]
        public UnitSpawnConfig[] unitsScenario;

        [Tooltip("Available object and their conditions of occurrence for this scenario.")]
        public ObjectSpawnConfig[] objectsScenario;

        [Tooltip("Available effects and their conditions of occurrence for this scenario.")]
        public EffectSpawnConfig[] effectsScenario;

        [Tooltip("Scenario spawn points prototypes.")]
        public GameObject spawnPoints;
    }
}