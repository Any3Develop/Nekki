using System;
using System.Collections.Generic;
using Nekki.Common.UIService.Data;

namespace Nekki.Common.UIService.Abstractions.AudioSource
{
    public interface IUIAudioSource : IDisposable
    {
        event Action<UIAudioClipData> OnPlayAudio; 
        IEnumerable<IUIAudioHandler> Handlers { get; }
        bool Enabled { get; }
        
        void Enable();
        void Disable();
    }
}