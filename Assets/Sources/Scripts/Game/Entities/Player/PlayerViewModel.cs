using Nekki.Common.InputSystem.Abstractions;
using Nekki.Game.Abilities.Abstractions;
using Nekki.Game.Entities.Abstractions;
using Nekki.Game.Entities.Player.Abstractions;
using Nekki.Game.Entities.Player.Data;
using Nekki.Game.Stats.Abstractions;
using Nekki.Game.Stats.Data;
using UnityEngine;
using IServiceProvider = Nekki.Common.DependencyInjection.IServiceProvider;

namespace Nekki.Game.Entities.Player
{
    public class PlayerViewModel : MonoBehaviour, IPlayerViewModel
    {
        [SerializeField] private bool is2D;
        [field:SerializeField] public GameObject Container { get; private set; }
        [field:SerializeField] public Transform Root { get; private set; }
        
        public PlayerConfig Config { get; private set; }
        public bool IsAlive => StatsCollection.TryGet(StatType.Heath, out var health) && health.Current > 0;
        public IPlayerView View { get; private set; }
        public IStatsCollection StatsCollection { get; private set; }
        public IAbilityCollection AbilityCollection { get; private set; }
        public IPlayerMovement Movement { get; private set; }
        public IPlayerAnimator Animator { get; private set; }
        public IInputController<PlayerActions> Input { get; private set; }
        
        public void Init(
            IServiceProvider serviceProvider, 
            IStatsCollection statsCollection, 
            IAbilityCollection abilityCollection,
            PlayerConfig config, 
            Vector3 position)
        {
            Config = config;
            StatsCollection = statsCollection;
            AbilityCollection = abilityCollection;
            View = serviceProvider.GetRequiredService<IPlayerViewFactory>().Create(this, Root);
            Animator = serviceProvider.GetRequiredService<IPlayerAnimatorFactory>().Create(this);
            Movement = serviceProvider.GetRequiredService<IPlayerMovementFactory>().Create(is2D, this, is2D ? GetComponent<Rigidbody2D>() : GetComponent<Rigidbody>(), position);
            Input = serviceProvider.GetRequiredService<IInputController<PlayerActions>>();
            
            abilityCollection.ForEach(x => x.ApplyOwnership(this));
            abilityCollection.ForEach(x => x.Enable(true));
            
            Animator.Enable(true);
            Movement.Enable(true);
            Input.Enable(true);
        }
        
        public bool ApplyDamage(float damage)
        {
            if (!IsAlive && damage <= 0)
                return false;
            
            if (StatsCollection.TryGet(StatType.Armor, out var armor))
                damage = Mathf.Max(damage - (damage * armor.Current), 0);
            
            Debug.Log($"{Config.name.Replace("Cfg", "")} : {nameof(ApplyDamage)}, damage : {damage}");
            
            if (StatsCollection.TryGet(StatType.Heath, out var health) && damage > 0)
            {
                health.Subtract(damage);
                View.Shake();
                TryDie();
                return true;
            }
            
            return false;
        }

        private void TryDie()
        {
            if (IsAlive)
                return;

            Input.Enable(false);
            Animator.Enable(false);
            Movement.Enable(false);
            AbilityCollection.ForEach(x => x.Enable(false));
            
            foreach (var meshRenderer in View.Mapper.GetAll<Renderer>(true))
                meshRenderer.material.color = Color.red;
        }
        
        private void OnDestroy()
        {
            View = null;
            Config = null;
            Movement?.Dispose();
            Animator?.Dispose();
            StatsCollection?.Clear();
            StatsCollection = null;
            Movement = null;
            Animator = null;
        }

        private void OnValidate()
        {
            if (!Root)
                Root = transform;

            if (!Container)
                Container = gameObject;
        }

        #region IRuntimeEntity Impl
        IRuntimeEntityView IRuntimeEntity.View => View;
        #endregion
    }
}