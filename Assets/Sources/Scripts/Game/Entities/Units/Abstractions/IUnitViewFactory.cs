namespace Nekki.Game.Entities.Units.Abstractions
{
    public interface IUnitViewFactory
    {
        IUnitView Create(IUnitViewModel value);
    }
}