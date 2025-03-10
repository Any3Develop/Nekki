using System.Collections.Generic;
using Nekki.Game.Context.Data.Spawn;
using UnityEngine;

namespace Nekki.Game.Entities.Player.Data
{
    [CreateAssetMenu(order = -1, fileName = "PlayerConfig", menuName = "Nekki/Player/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [Header("General")]
        [Tooltip("Which scenario code will execute with this config.")]
        public PlayerId id;
        
        [Space, Header("Prototypes")]
        public PlayerView viewPrefab;
        public PlayerViewModel viewModelPrefab;
        
        [Space, Header("Spawn")]
        [Tooltip("Provide how to select from the list of PositionIds a next spawn id.")]
        public FunctionSelector spawnFunction;

        [Tooltip("At what points can player of this config spawn?")]
        public List<SpawnId> spawnIds = new();
    }
}