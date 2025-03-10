using System;
using Nekki.Common.Events;
using Nekki.Game.Abilities.Abstractions;
using Nekki.Game.Abilities.Events;
using UnityEngine;

namespace Nekki.Game.Abilities.Views
{
    public class AbilityMovement : MonoBehaviour, IAbilityMovement
    {
        private bool hasSteering;
        private bool hasDirection;
        private bool hasDestination;
        private Vector3 initPosition;

        [field: SerializeField] protected Transform Root { get; private set; }
        [field: SerializeField] protected float StopDistance { get; private set; }
        protected IAbilityView View { get; private set; }
        protected float Speed { get; private set; }
        protected float? RangeLimit { get; private set; }
        protected float RangePassed { get; private set; }
        protected float DistanceLeft { get; private set; }
        protected Vector3 Direction { get; private set; }
        protected Vector3 Destination { get; private set; }
        protected Transform Target { get; private set; }
        protected bool Enabled { get; private set; }

        private void Awake() => View = GetComponent<IAbilityView>();

        public void Apply(float speed, float? rangeLimit = null)
        {
            if (!hasSteering && !hasDirection && !hasDestination)
                throw new InvalidOperationException($"Select one of the following fluid interfaces before {nameof(Apply)}: {nameof(SetSteering)}, {nameof(SetDirection)}, {nameof(SetDestination)}.");

            if (!hasSteering)
                Target = null;

            if (!hasDestination)
                Destination = Vector3.zero;

            if (!hasDirection)
                Direction = Vector3.zero;

            Speed = speed;
            RangeLimit = rangeLimit;
            Enabled = true;
        }

        public void Reset()
        {
            if (!Enabled)
                return;

            Enabled = false;
            Target = null;
            Direction = Root.forward;
            Destination = Vector3.zero;
            Speed = RangePassed = 0;
            RangeLimit = null;
            hasSteering = false;
            hasDirection = false;
            hasDestination = false;
        }

        public void Enable(bool value)
        {
            if (Enabled == value || (!hasDirection && !hasDestination && !hasSteering))
                return;

            Enabled = value;
        }

        public IAbilityMovement SetPosition(Vector3 value)
        {
            Root.position = value;
            return this;
        }

        public IAbilityMovement SetRotation(Quaternion value)
        {
            Root.rotation = value;
            return this;
        }

        public IAbilityMovement SetSteering(Transform value)
        {
            Enabled = false;
            Target = value;
            hasSteering = true;
            return this;
        }

        public IAbilityMovement SetDirection(Vector3 value)
        {
            Enabled = false;
            Direction = value.normalized;
            hasDirection = true;
            return this;
        }

        public IAbilityMovement SetDestination(Vector3 value)
        {
            Enabled = false;
            Destination = value;
            hasDestination = true;
            return this;
        }

        protected void LateUpdate()
        {
            if (!Enabled)
                return;

            if (hasDirection)
            {
                Root.position += Direction * (Speed * Time.deltaTime);
                ProcessPassedPath();
                return;
            }

            if (hasSteering && Target)
                Destination = Target.position;

            Root.position = Vector3.MoveTowards(Root.position, Destination, Speed * Time.deltaTime);
            ProcessPassedPath();
        }

        private void ProcessPassedPath()
        {
            var pos = Root.position;
            RangePassed = Vector3.Distance(pos, initPosition);
            DistanceLeft = hasDestination ? Vector3.Distance(pos, Destination) : float.PositiveInfinity;
            if (!RangeLimit.HasValue || RangePassed < RangeLimit || DistanceLeft > StopDistance)
                return;

            Enable(false);
            MessageBroker.Publish(new AbilityDestinationReachedEvent(View));
        }

        private void OnValidate()
        {
            if (!Root)
                Root = transform;
        }
    }
}