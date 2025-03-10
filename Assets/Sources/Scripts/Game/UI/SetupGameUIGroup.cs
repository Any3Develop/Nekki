using System;
using Nekki.Common.Audio;
using Nekki.Common.LifecycleService;
using Nekki.Common.UIService;
using Nekki.Common.UIService.Abstractions;

namespace Nekki.Game.UI
{
    public class SetupGameUIGroup : IInitable, IDisposable
    {
        private readonly IUIService uiService;
        private readonly IUIAudioListener audioListener;

        public SetupGameUIGroup(IUIService uiService, IUIAudioListener audioListener)
        {
            this.uiService = uiService;
            this.audioListener = audioListener;
        }

        public void Initialize()
        {
            audioListener.Subscribe(uiService.CreateAll(UILayer.GameUIGroup));
        }

        public void Dispose()
        {
            uiService.DestroyAll(UILayer.GameUIGroup);
        }
    }
}