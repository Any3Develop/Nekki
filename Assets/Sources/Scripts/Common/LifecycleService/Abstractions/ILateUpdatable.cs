namespace Nekki.Common.LifecycleService
{
    public interface ILateUpdatable : ILifecycleObject
    {
        void LateUpdate();
    }
}