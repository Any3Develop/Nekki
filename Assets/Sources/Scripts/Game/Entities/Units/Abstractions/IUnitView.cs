using Nekki.Common.Pools.Abstractions;
using Nekki.Game.Entities.Abstractions;

namespace Nekki.Game.Entities.Units.Abstractions
{
    public interface IUnitView : IRuntimeEntityView, ISpwanPoolable
    {
        IUnitViewModel ViewModel { get; }
        void Shake();
    }
}