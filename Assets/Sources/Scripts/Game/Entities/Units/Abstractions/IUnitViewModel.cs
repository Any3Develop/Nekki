using Nekki.Common.Pools.Abstractions;
using Nekki.Game.Context.Abstractions;
using Nekki.Game.Entities.Abstractions;
using Nekki.Game.Entities.Units.Data;

namespace Nekki.Game.Entities.Units.Abstractions
{
    public interface IUnitViewModel : IRuntimeEntity, ISpwanPoolable
    {
        UnitConfig Config { get; }
        new IUnitView View { get; }
        IUnitMovement Movement { get; }
        IGameContext GameContext { get; }
    }
}