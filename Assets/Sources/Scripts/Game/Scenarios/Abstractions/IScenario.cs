using System;
using System.Collections.Generic;
using Nekki.Game.Scenarios.Data;

namespace Nekki.Game.Scenarios.Abstractions
{
    public interface IScenario : IDisposable
    {
        ScenarioState State { get; }
        IEnumerable<IScenario> Nested { get; }
        
        void Start();
        void Pause(bool value);
        void End();
    }
}