namespace Nekki.Common.DependencyInjection
{
    public interface IServiceProvider
    {
        T GetRequiredService<T>();
    }
}