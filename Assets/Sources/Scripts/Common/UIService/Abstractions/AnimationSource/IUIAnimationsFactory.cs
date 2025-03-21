namespace Nekki.Common.UIService.Abstractions.AnimationSource
{
    public interface IUIAnimationsFactory
    {
        IUIAnimation Create(IUIWindow window, IUIAnimationConfig config);
    }
}