using Nekki.Common.SceneService;
using Nekki.Infrastructure.Common;
using Nekki.Lobby.UI;
using Nekki.Lobby.UI.GameModes;
using Zenject;

namespace Nekki.Infrastructure.Lobby
{
    public class LobbyInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            ProjectContextInstaller.InstallSettings();
            Container
                .BindInterfacesTo<SetupLobbyUIGroup>()
                .AsSingle()
                .NonLazy();
            
            Container
                .BindInterfacesTo<GameModesViewModel>()
                .AsSingle()
                .WithArguments(new GameModeData[]
                {
                    new(SceneId.Demo3D, "3D Game"),
                    new(SceneId.Demo2D, "2D Game"),
                })
                .NonLazy();
        }
    }
}