﻿using UnityEngine;

namespace Nekki.Common.UIService.Abstractions
{
    public interface IUIWindowFactory
    {
        T Create<T>(string groupId, Transform parent = null) where T : IUIWindow;
        IUIWindow[] Create(string groupId, Transform parent = null);
        IUIWindow[] CreateAll(Transform parent = null);
    }
}