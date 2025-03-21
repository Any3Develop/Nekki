﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Nekki.Common.UIService.Abstractions.AnimationSource
{
    public interface IUIAnimationSource : IDisposable
    {
        bool Enabled { get; }
        IEnumerable<IUIAnimation> Animations { get; }
        
        void Enable(bool value);
        Task ExecuteAsync(object id, CancellationToken token);
    }
}