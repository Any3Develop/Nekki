using Nekki.Common.Pools.Abstractions;
using Nekki.Game.Abilities.Abstractions;
using Nekki.Game.Abilities.Data;
using Nekki.Game.Context.Abstractions;
using Nekki.Game.Entities.Abstractions;
using Nekki.Game.Stats.Abstractions;
using Nekki.Game.Stats.Utils;

namespace Nekki.Game.Abilities
{
    public abstract class AbilityBase : IAbility
    {
        protected IPool<IAbilityView> ViewPool {get; private set;}
        protected IAbilityViewFactory ViewFactory { get; private set; }
        protected IGameContext GameContext { get; private set; }

        public bool Enabled { get; private set; }
        public AbilityConfig Config { get; private set;}
        public IRuntimeEntity Owner { get; private set;}
        public IStatsCollection StatsCollection { get; private set;}

        public void Init(
            AbilityConfig config, 
            IGameContext gameContext)
        {
            Config = config;
            GameContext = gameContext;
            ViewPool = gameContext.ServiceProvider.GetRequiredService<IPool<IAbilityView>>();
            StatsCollection = gameContext.ServiceProvider.GetRequiredService<IStatsCollection>().Merge(config.stats);
            ViewFactory = gameContext.ServiceProvider.GetRequiredService<IAbilityViewFactory>();
            OnInit();
        }
        
        public void ApplyOwnership(IRuntimeEntity value)
        {
            var previous = Owner;
            Owner = value;
            OnOwnershipChanged(previous);
        }

        public void Enable(bool value)
        {
            if (Enabled == value)
                return;

            Enabled = value;
            if (Enabled)
            {
                OnEnabled();
                return;
            }
            
            OnDisabled();
        }

        public virtual bool CanExecute() => Enabled;

        public void Execute()
        {
            if (!CanExecute())
                return;

            OnExecute();
        }
        
        protected virtual void OnInit(){}
        protected virtual void OnEnabled(){}
        protected virtual void OnDisabled(){}
        protected virtual void OnOwnershipChanged(IRuntimeEntity previous){}
        protected abstract void OnExecute();
    }
}