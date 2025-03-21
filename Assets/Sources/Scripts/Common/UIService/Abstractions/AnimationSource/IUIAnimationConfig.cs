﻿using UnityEngine;

namespace Nekki.Common.UIService.Abstractions.AnimationSource
{
    public interface IUIAnimationConfig
    {
        public bool EnabledByDefault {get;}
        public bool ReInitWhenModified {get;}
        public AnimationCurve EaseCurve {get;}

        public bool HasPlayEvent(object id);
        public bool HasStopEvent(object id);
        public bool HasResetEvent(object id);
    }
}