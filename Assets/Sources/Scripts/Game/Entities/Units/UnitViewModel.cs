using Nekki.Common.Events;
using Nekki.Common.Pools;
using Nekki.Common.Pools.Abstractions;
using Nekki.Game.Abilities.Abstractions;
using Nekki.Game.Context.Abstractions;
using Nekki.Game.Entities.Abstractions;
using Nekki.Game.Entities.Units.Abstractions;
using Nekki.Game.Entities.Units.Data;
using Nekki.Game.Entities.Units.Events;
using Nekki.Game.Stats.Abstractions;
using Nekki.Game.Stats.Data;
using Nekki.Game.Stats.Utils;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using IServiceProvider = Nekki.Common.DependencyInjection.IServiceProvider;

namespace Nekki.Game.Entities.Units
{
    public class UnitViewModel : PoolableView, IUnitViewModel
    {
        [field:SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }
        [field:SerializeField] public BehaviorGraphAgent BehaviorGraphAgent { get; private set; }
        
        #region Dependencies
        public bool IsAlive => StatsCollection.TryGet(StatType.Heath, out var health) && health.Current > 0;
        public IUnitView View { get; private set; }
        public UnitConfig Config { get; private set; }
        public IStatsCollection StatsCollection { get; private set; }
        public IAbilityCollection AbilityCollection { get; private set; }
        public IUnitMovement Movement { get; private set; }
        public IGameContext GameContext { get; private set; }
        public IUnitViewFactory ViewFactory { get; private set; }
        public IPool<IUnitView> ViewPool { get; private set; }
        public IPool<IUnitViewModel> ViewModelPool { get; private set; }

        #endregion

        public void Construct(IServiceProvider serviceProvider)
        {
            if (DisposedLog())
                return;

            GameContext = serviceProvider.GetRequiredService<IGameContext>();
            StatsCollection = serviceProvider.GetRequiredService<IStatsCollection>();
            ViewFactory = serviceProvider.GetRequiredService<IUnitViewFactory>();
            ViewPool = serviceProvider.GetRequiredService<IPool<IUnitView>>();
            ViewModelPool = serviceProvider.GetRequiredService<IPool<IUnitViewModel>>();
            Movement = serviceProvider.GetRequiredService<IUnitMovementFactory>().Create(NavMeshAgent, this);
            AbilityCollection = serviceProvider.GetRequiredService<IAbilityCollection>();
        }

        public void Init(UnitConfig config, Vector3 position)
        {
            if (DisposedLog())
                return;

            Config = config;
            StatsCollection.Merge(config.stats);
            View = ViewFactory.Create(this);
            
            AbilityCollection.ForEach(x=> x.ApplyOwnership(this));
            AbilityCollection.ForEach(x => x.Enable(true));
            
            if (BehaviorGraphAgent)
            {
                BehaviorGraphAgent.End();
                BehaviorGraphAgent.Graph = Config.behavioursSet.Get(config.behaviorType);
                BehaviorGraphAgent.Init();
            }

            OnInit(position);
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
            if (!StatsCollection.TryGet(StatType.Heath, out var health) || health.Current > 0)
                return;
            
            MessageBroker.Publish(new UnitDiedEvent());
            ViewModelPool.Release(this);
        }

        protected virtual void OnInit(Vector3 position)
        {
            Movement.Move(position);
            var root = View.Mapper.Map<Transform>("Root");
            root.SetParent(Root);
            root.localPosition = Vector3.zero;
            root.localRotation = Quaternion.identity;
        }

        protected override void OnSpawned()
        {
            Movement.Enable(true);
            
            MessageBroker.Publish(new UnitSpawnedEvent());
            
            if (BehaviorGraphAgent)
                BehaviorGraphAgent.Start();
        }

        protected override void OnReleased()
        {
            if (BehaviorGraphAgent)
            {
                BehaviorGraphAgent.End();
                if (BehaviorGraphAgent.Graph)
                    DestroyImmediate(BehaviorGraphAgent.Graph);

                BehaviorGraphAgent.Graph = null;
            }
            
            AbilityCollection.ForEach(x => x.Enable(false));
            Movement.Enable(false);
            ViewPool.Release(View);
            StatsCollection.Clear();
            
            Config = null;
            View = null;
        }

        protected override void OnDisposed()
        {
            GameContext = null;
            StatsCollection = null;
            ViewFactory = null;
            ViewPool = null;
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            if (!NavMeshAgent)
                NavMeshAgent = GetComponent<NavMeshAgent>();

            if (!BehaviorGraphAgent)
                BehaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
        }

        #region IRuntimeEntityView Impl
        IRuntimeEntityView IRuntimeEntity.View => View;
        #endregion
    }
}