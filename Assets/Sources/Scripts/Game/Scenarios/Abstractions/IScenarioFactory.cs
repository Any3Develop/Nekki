using Nekki.Game.Scenarios.Data;

namespace Nekki.Game.Scenarios.Abstractions
{
    public interface IScenarioFactory
    {
        IScenario Create(ScenarioId id, params object[] args);
    }
}