﻿using System.Threading;
using System.Threading.Tasks;
using Nekki.Common.UIService.Abstractions.AnimationSource;
using Nekki.Common.UIService.AnimationSource.Configs;

namespace Nekki.Common.UIService.AnimationSource.Animations
{
    public sealed class UINoAnimation : IUIAnimation
    {
        public IUIAnimationConfig Configuration { get; } = new UINoAnimationConfig();
        
        public bool Enabled { get; private set; }

        public void Dispose() => Disable();
        
        public void Enable() => Enabled = true;

        public void Disable() => Enabled = false;

        public Task PlayAsync(CancellationToken token) => Task.CompletedTask;

        public Task StopAsync(CancellationToken token) => Task.CompletedTask;

        public Task ResetAsync(CancellationToken token) => Task.CompletedTask;
    }
}