﻿using UnityEngine;

namespace Nekki.Common.UIService.Abstractions.FullFade
{
    public interface IFullFadeColor : IFullFadeTarget
    {
        Color FadeColor { get; }
    }
}