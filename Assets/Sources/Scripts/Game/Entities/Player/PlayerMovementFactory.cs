using Nekki.Common.DependencyInjection;
using Nekki.Game.Entities.Player.Abstractions;

namespace Nekki.Game.Entities.Player
{
    public class PlayerMovementFactory : IPlayerMovementFactory
    {
        private readonly IAbstractFactory abstractFactory;
        public PlayerMovementFactory(IAbstractFactory abstractFactory)
        {
            this.abstractFactory = abstractFactory;
        }

        public IPlayerMovement Create(bool is2D, params object[] args)
        {
            return is2D 
                ? abstractFactory.Create<PlayerMovement2D>(args) 
                : abstractFactory.Create<PlayerMovement>(args);
        }
    }
}