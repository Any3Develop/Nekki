using System;
using Nekki.Common.UIService.Data;

namespace Nekki.Common.UIService.Abstractions.AudioSource
{
    public interface IUIAudioHandler : IDisposable
    {
        event Action<UIAudioClipData> OnPayAudio;
        bool Enabled { get; }
        void Enable();
        void Disable();
    }
}