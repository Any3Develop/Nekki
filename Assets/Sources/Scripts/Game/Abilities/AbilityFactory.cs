using System;
using System.Collections.Generic;
using System.Linq;
using Nekki.Common.Collections;
using Nekki.Common.DependencyInjection;
using Nekki.Game.Abilities.Abstractions;
using Nekki.Game.Abilities.Data;
using Nekki.Game.Context.Abstractions;
using Nekki.Game.Stats.Utils;
using UnityEngine;

namespace Nekki.Game.Abilities
{
    public class AbilityFactory : IAbilityFactory
    {
        private readonly IAbstractFactory abstractFactory;
        private readonly IGameContext gameContext;
        private readonly TypeCollection<AbilityId,  AbilityAttribute> typeCollection;
        private readonly Dictionary<AbilityId, AbilityConfig> configStorage;

        public AbilityFactory(IAbstractFactory abstractFactory, IGameContext gameContext)
        {
            this.abstractFactory = abstractFactory;
            this.gameContext = gameContext;
            typeCollection = new TypeCollection<AbilityId, AbilityAttribute>(att => att.Id, typeof(IAbility));
            configStorage = Resources.LoadAll<AbilityConfig>("Game/AbilityConfigs").ToDictionary(x => x.id);
        }
        
        public IAbility Create(AbilityId id)
        {
            if (!typeCollection.TryGet(id, out var type))
                throw new NullReferenceException($"Can't create a {nameof(IAbility)} with id : {id}, because it's not registered.");

            var ability = (AbilityBase)abstractFactory.Create(type);
            ability.Init(configStorage[id], gameContext);
            return ability;
        }

        public IAbility Create(AbilityData data)
        {
            var ability = Create(data.Id);
            ability.StatsCollection.Merge(data.Stats);
            return ability;
        }
    }
}