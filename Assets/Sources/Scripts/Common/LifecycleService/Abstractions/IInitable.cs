namespace Nekki.Common.LifecycleService
{
    public interface IInitable : ILifecycleObject
    {
        void Initialize();
    }
}