using System;
using System.Linq;
using Nekki.Common.Events;
using Nekki.Game.Abilities.Abstractions;
using Nekki.Game.Abilities.Data;
using Nekki.Game.Abilities.Events;
using Nekki.Game.Abilities.Views;
using Nekki.Game.Entities.Abstractions;
using Nekki.Game.Stats.Abstractions;
using Nekki.Game.Stats.Data;
using R3;
using UnityEngine;

namespace Nekki.Game.Abilities.ViewModels
{
    [Ability(AbilityId.BattleAbility0)]
    [Ability(AbilityId.BattleAbility1)]
    [Ability(AbilityId.BattleAbility2)]
    [Ability(AbilityId.BattleAbility3)]
    [Ability(AbilityId.BattleAbility4)]
    [Ability(AbilityId.BattleAbility5)] // Different ids but one logic, sample)
    public class AbilityProjectile : AbilityBase
    {
        private IDisposable subscribes;
        private IRuntimeStat countDown;
        private IRuntimeStat moveSpeed;
        private IRuntimeStat rangeLimit;
        private IRuntimeStat damage;
        private LayerMask ignorLayer;
        private Transform startPoint;
        private float lastUse;

        protected override void OnEnabled()
        {
            // TODO Apply owner's int pow, countdown reduction, attack range, etc. Or move it to another part to make it generic logic.
            countDown = StatsCollection.Get(StatType.Countdown);
            moveSpeed = StatsCollection.Get(StatType.MoveSpeed);
            rangeLimit = StatsCollection.Get(StatType.Range);
            damage = StatsCollection.Get(StatType.Damage);

            using var builder = new DisposableBuilder();
            builder.Add(MessageBroker.Receive<AbilityDestinationReachedEvent>() // Out of ability range
                .Where(x => x.View?.Ability == this)
                .Subscribe(evData => DespawnView(evData.View)));

            builder.Add(MessageBroker.Receive<AbilityCollisionEvent>()
                .Where(x => x.View?.Ability == this)
                .Subscribe(evData => HandleCollision(evData.View, evData.GameObject)));

            subscribes = builder.Build();
        }

        protected override void OnDisabled()
        {
            DespawnAll();
            subscribes?.Dispose();
            subscribes = null;
            countDown = null;
            moveSpeed = null;
            rangeLimit = null;
            damage = null;
        }

        protected override void OnOwnershipChanged(IRuntimeEntity previous)
        {
            if (Owner != previous)
                DespawnAll();

            if (Owner == null)
                return;

            lastUse = 0;
            Owner.View.Mapper.TryMap("Spells_StartPoint", out startPoint);
            ignorLayer = (LayerMask) Owner.StatsCollection.Get(StatType.IgnoreLayers).Current;
        }

        public override bool CanExecute() => base.CanExecute()
                                             && Owner is {IsAlive: true}
                                             && lastUse < Time.time;

        protected override void OnExecute()
        {
            lastUse = Time.time + countDown.Current;
            var view = ViewFactory.Create<AbilityProjectileView>(this);
            view.ExcludeCollisionLayers(ignorLayer);
            view.Movement
                .SetDirection(startPoint.forward)
                .SetPosition(startPoint.position)
                .SetRotation(startPoint.rotation)
                .Apply(moveSpeed.Current, rangeLimit.Current);
        }

        private void HandleCollision(IAbilityView view, GameObject other)
        {
            DespawnView(view);

            if (!other.TryGetComponent(out IRuntimeEntity entity))
                return;

            entity.ApplyDamage(damage.Current);
        }

        private void DespawnView(IAbilityView view)
        {
            ViewPool.Release(view);
        }

        private void DespawnAll()
        {
            ViewPool.Release(x => x.Ability == this);
        }
    }
}