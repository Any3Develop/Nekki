﻿#if !DOTWEEN
using System;
using System.Collections;
using System.Threading;
using Nekki.Common.UIService.AnimationSource.Configs;
using UnityEngine;

namespace Nekki.Common.UIService.AnimationSource.Animations
{
    public class UIPopupUnityAnimation : UIAnimationBase<UIPopupAnimationConfig>
    {
        private Coroutine tween;

        protected override void OnPlay(Action onCompleted, CancellationToken token)
        {
            OnReset(() => tween = UIAnimationCoroutineRunner.StartCoroutine(OnPlayAsync(onCompleted, token)), token);
        }

        private IEnumerator OnPlayAsync(Action onCompleted, CancellationToken token)
        {
	        var content = Window?.Content;
	        if (!content || token.IsCancellationRequested)
		        EndAnimation();
	        
            if (Config.Delay > 0)
                yield return new WaitForSeconds(Config.Delay);
            
            if (!content || token.IsCancellationRequested)
                EndAnimation();
            
            var elapsedTime = 0f;
            var duration = Config.Duration;
            var ease = Config.EaseCurve;
            var from = content!.localScale;
            var to = Config.ToSize.GetAllowed(from);
            while (Application.isPlaying && elapsedTime < duration && content && !token.IsCancellationRequested)
            {
                elapsedTime += Time.deltaTime;
                var t = ease.Evaluate(elapsedTime / duration);

                if (!content || token.IsCancellationRequested)
                    EndAnimation();

                content.localScale = Vector3.LerpUnclamped(from, to, t);
                yield return null;
            }

            EndAnimation(to);
            
            yield break;
            void EndAnimation(Vector3? end = default)
            {
                if (end.HasValue && content)
                    content.localScale = end.Value;
                
                OnStop(null, token);
                onCompleted?.Invoke();
            }
        }
        
        protected override void OnStop(Action onCompleted, CancellationToken token)
        {
            UIAnimationCoroutineRunner.StopCoroutine(tween);
            tween = null;
            onCompleted?.Invoke();
        }
        
        protected override void OnReset(Action onCompleted, CancellationToken token)
        {
            OnStop(null, token);
            if (!Window?.Content)
            {
                onCompleted?.Invoke();
                return;
            }

            var destination = Config.FromSize.GetAllowed(Window.Content.localScale);
            Window.Content.localScale = destination;
            onCompleted?.Invoke();
        }

        protected override void OnDisposed() => OnStop(null, CancellationToken.None);
    }
}
#endif