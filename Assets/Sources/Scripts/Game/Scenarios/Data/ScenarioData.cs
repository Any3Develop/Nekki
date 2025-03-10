using Nekki.Lobby.Identity.Abstractions;

namespace Nekki.Game.Scenarios.Data
{
    public class ScenarioData : IRedirectionArg
    {
        public ScenarioId Id { get; set; }
        public int Level { get; set; }
    }
}