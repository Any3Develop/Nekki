using System;
using Nekki.Game.Abilities.Data;

namespace Nekki.Game.Abilities
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AbilityAttribute : Attribute
    {
        public AbilityId Id { get; }
        public AbilityAttribute(AbilityId id) => Id = id;
    }
}