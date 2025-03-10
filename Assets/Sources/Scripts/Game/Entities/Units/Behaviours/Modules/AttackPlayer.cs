using System;
using Nekki.Game.Entities.Units.Abstractions;
using Nekki.Game.Stats.Data;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Nekki.Game.Entities.Units.Behaviours.Modules
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "AttackPlayer", story: "Agent starts an attack", category: "Action", id: "103ac4c6d1db39a507978a7c2b5c2b99")]
    public partial class AttackPlayer : Action
    {
        private IUnitViewModel unit;
        private float attackDelay;
        private float attackDamage;
        private float lastHit;

        protected override Status OnStart()
        {
            unit ??= GameObject.GetComponent<IUnitViewModel>();
            attackDelay = unit.StatsCollection.Get(StatType.StartDelay).Current;
            attackDamage = unit.StatsCollection.Get(StatType.Damage).Current;
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            if (lastHit > Time.time || !unit.GameContext.Player.IsAlive)
                return Status.Success;
            
            lastHit = attackDelay + Time.time;
            unit.GameContext.Player?.ApplyDamage(attackDamage);
            return Status.Success;
        }
    }
}

