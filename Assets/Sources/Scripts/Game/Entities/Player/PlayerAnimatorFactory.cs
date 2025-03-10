using Nekki.Common.DependencyInjection;
using Nekki.Game.Entities.Player.Abstractions;

namespace Nekki.Game.Entities.Player
{
    public class PlayerAnimatorFactory : IPlayerAnimatorFactory
    {
        private readonly IAbstractFactory abstractFactory;
        public PlayerAnimatorFactory(IAbstractFactory abstractFactory)
        {
            this.abstractFactory = abstractFactory;
        }

        public IPlayerAnimator Create(params object[] args)
        {
            return abstractFactory.Create<PlayerAnimator>(args);
        }
    }
}