using Nekki.Game.Abilities.Abstractions;

namespace Nekki.Game.Abilities.Events
{
    public readonly struct AbilityDestinationReachedEvent
    {
        public IAbilityView View { get; }
        
        public AbilityDestinationReachedEvent(IAbilityView view) => View = view;
    }
}