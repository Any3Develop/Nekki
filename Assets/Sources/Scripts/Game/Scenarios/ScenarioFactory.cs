using System;
using Nekki.Common.Collections;
using Nekki.Common.DependencyInjection;
using Nekki.Game.Scenarios.Abstractions;
using Nekki.Game.Scenarios.Data;

namespace Nekki.Game.Scenarios
{
    public class ScenarioFactory : IScenarioFactory
    {
        private readonly IAbstractFactory abstractFactory;
        private readonly TypeCollection<ScenarioId, ScenarioAttribute> typeCollection;

        public ScenarioFactory(IAbstractFactory abstractFactory)
        {
            this.abstractFactory = abstractFactory;
            typeCollection = new TypeCollection<ScenarioId, ScenarioAttribute>(att => att.Id, typeof(IScenario));
        }
        
        public IScenario Create(ScenarioId id, params object[] args)
        {
            if (!typeCollection.TryGet(id, out var type))
                throw new NullReferenceException($"Can't create a {nameof(IScenario)} with id : {id}, because it's not registered.");

            return (IScenario)abstractFactory.Create(type, args);
        }
    }
}