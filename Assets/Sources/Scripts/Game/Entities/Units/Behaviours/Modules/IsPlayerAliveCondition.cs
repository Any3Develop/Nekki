using System;
using Nekki.Game.Entities.Units.Abstractions;
using Unity.Behavior;

namespace Nekki.Game.Entities.Units.Behaviours.Modules
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(name: "IsPlayerAlive", story: "Is Player Alive?", category: "Conditions", id: "40c8f1fc5ee0c3d4ceb5797c8ede0d1d")]
    public partial class IsPlayerAliveCondition : Condition
    {
        private IUnitViewModel unit;
        
        public override void OnStart()
        {
            unit ??= GameObject.GetComponent<IUnitViewModel>();
            base.OnStart();
        }

        public override bool IsTrue()
        {
            return unit?.GameContext.Initialized == true && unit.GameContext.Player.IsAlive;
        }
    }
}
