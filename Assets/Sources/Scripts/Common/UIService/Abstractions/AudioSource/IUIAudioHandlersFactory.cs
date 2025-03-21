﻿namespace Nekki.Common.UIService.Abstractions.AudioSource
{
    public interface IUIAudioHandlersFactory
    {
        IUIAudioHandler Create(IUIWindow window, IUIAudioConfig config);
    }
}