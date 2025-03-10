using Nekki.Common.UIService.Abstractions;
using Nekki.Common.UIService.Abstractions.AnimationSource;
using Nekki.Common.UIService.AnimationSource.Animations;
using Nekki.Common.UIService.AnimationSource.Configs;

namespace Nekki.Common.UIService.AnimationSource.Factories
{
    public class UIAnimationsFactory : IUIAnimationsFactory
    {
        public virtual IUIAnimation Create(IUIWindow window, IUIAnimationConfig config)
        {
            return config switch
            {
#if DOTWEEN
                UIPopupAnimationConfig => new UIPopupDoTweenAnimation().Init(window, config),
                UIAppearAnimationConfig => new UIAppearDoTweenAnimation().Init(window, config),
                UIFadeAnimationConfig => new UIFadeDoTweenAnimation().Init(window, config),
#else
                UIPopupAnimationConfig => new UIPopupUnityAnimation().Init(window, config),
                UIAppearAnimationConfig => new UIAppearUnityAnimation().Init(window, config),
                UIFadeAnimationConfig => new UIFadeUnityAnimation().Init(window, config),
#endif

                _ => new UINoAnimation()
            };
        }
    }
}