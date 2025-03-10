namespace Nekki.Game.Abilities.Abstractions
{
    public interface IAbilityViewFactory
    {
        IAbilityView Create(IAbility ability, int index = 0);
        TView Create<TView>(IAbility ability, int index = 0) where TView : IAbilityView;
    }
}