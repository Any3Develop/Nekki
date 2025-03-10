using Nekki.Common.SceneService;

namespace Nekki.Lobby.UI.GameModes
{
    public readonly struct GameModeData
    {
        public SceneId Id { get; }
        public string Name { get; }

        public GameModeData(SceneId id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}