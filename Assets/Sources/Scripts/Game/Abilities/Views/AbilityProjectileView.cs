using Nekki.Common.Events;
using Nekki.Game.Abilities.Abstractions;
using Nekki.Game.Abilities.Events;
using UnityEngine;

namespace Nekki.Game.Abilities.Views
{
    [RequireComponent(typeof(IAbilityMovement))]
    public class AbilityProjectileView : AbilityBaseView
    {
        [Space, Header("Custom")]
        [SerializeField] private Collider coll3D;
        [SerializeField] private Collider2D coll2D;
        private LayerMask defaultMask;
        public IAbilityMovement Movement { get; private set; }

        private void Awake()
        {
            Movement = GetComponent<IAbilityMovement>();
            defaultMask = coll3D ? coll3D.excludeLayers : coll2D.excludeLayers;
        }

        private void OnCollisionEnter(Collision other) => MessageBroker.Publish(new AbilityCollisionEvent(this, other.gameObject));
        private void OnCollisionEnter2D(Collision2D other) => MessageBroker.Publish(new AbilityCollisionEvent(this, other.gameObject));

        public void ExcludeCollisionLayers(LayerMask value)
        {
            if (coll3D)
                coll3D.excludeLayers = defaultMask | value;
            
            if (coll2D)
                coll2D.excludeLayers = defaultMask | value;
        }
        
        protected override void OnReleased()
        {
            base.OnReleased();
            Movement.Reset();
        }

        protected override void OnDisposed()
        {
            base.OnDisposed();
            Movement = null;
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            if (!coll3D)
                coll3D = GetComponent<Collider>();
            if (!coll2D)
                coll2D = GetComponent<Collider2D>();
        }
    }
}