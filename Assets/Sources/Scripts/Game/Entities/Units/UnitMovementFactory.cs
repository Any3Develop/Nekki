using Nekki.Common.DependencyInjection;
using Nekki.Game.Entities.Units.Abstractions;

namespace Nekki.Game.Entities.Units
{
    public class UnitMovementFactory : IUnitMovementFactory
    {
        private readonly IAbstractFactory abstractFactory;

        public UnitMovementFactory(IAbstractFactory abstractFactory)
        {
            this.abstractFactory = abstractFactory;
        }

        public IUnitMovement Create(params object[] args)
        {
            return abstractFactory.Create<UnitNavmeshMovement>(args);
        }
    }
}