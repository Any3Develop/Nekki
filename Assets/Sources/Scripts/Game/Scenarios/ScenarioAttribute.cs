using System;
using Nekki.Game.Scenarios.Data;

namespace Nekki.Game.Scenarios
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ScenarioAttribute : Attribute
    {
        public ScenarioId Id { get; }
        public ScenarioAttribute(ScenarioId id) => Id = id;
    }
}