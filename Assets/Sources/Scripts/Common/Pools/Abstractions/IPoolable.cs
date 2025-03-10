using System;

namespace Nekki.Common.Pools.Abstractions
{
    public interface IPoolable : IDisposable
    {
        void Release();
    }
}