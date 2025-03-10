using System.Collections.Generic;
using Nekki.Game.Abilities.Data;
using Nekki.Game.Stats.Data;
using Nekki.Lobby.Identity.Abstractions;

namespace Nekki.Game.Entities.Player.Data
{
    public class PlayerData : IRedirectionArg
    {
        public PlayerId Id { get; set; }
        public List<StatData> Stats { get; set; } = new();
        public List<AbilityData> Abilities { get; set; } = new();
    }
}