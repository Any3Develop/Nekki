using System;
using Nekki.Common.Audio;
using Nekki.Common.LifecycleService;
using Nekki.Common.UIService;
using Nekki.Common.UIService.Abstractions;

namespace Nekki.Lobby.UI
{
    public class SetupLobbyUIGroup : IInitable, IDisposable
    {
        private readonly IUIService uiService;
        private readonly IUIAudioListener audioListener;

        public SetupLobbyUIGroup(IUIService uiService, IUIAudioListener audioListener)
        {
            this.uiService = uiService;
            this.audioListener = audioListener;
        }

        public void Initialize()
        {
            audioListener.Subscribe(uiService.CreateAll(UILayer.LobbyUIGroup));
        }

        public void Dispose()
        {
            uiService.DestroyAll(UILayer.LobbyUIGroup);
        }
    }
}