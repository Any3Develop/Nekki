using Nekki.Game.Entities.Player.Abstractions;
using UnityEngine;

namespace Nekki.Game.Entities.Player
{
    public class PlayerAnimator : IPlayerAnimator
    {
        private readonly Animator animator;
        private static readonly int VelocityX = Animator.StringToHash(nameof(VelocityX));
        private static readonly int VelocityY = Animator.StringToHash(nameof(VelocityY));
        private static readonly int ActionType = Animator.StringToHash(nameof(ActionType));
        private static readonly int ActionTrigger = Animator.StringToHash(nameof(ActionTrigger));
        
        public IPlayerViewModel ViewModel { get; }
        public bool Enabled { get; private set; }

        public PlayerAnimator(IPlayerViewModel viewModel)
        {
            ViewModel = viewModel;
            animator = ViewModel.View.Mapper.Map<Animator>();
            if (!animator || !animator.runtimeAnimatorController)
                Debug.LogWarning($"{nameof(IPlayerAnimator)} is disabled because {nameof(Animator)} component doesn't exist or not initialized with controller.");
        }

        public void Dispose(){}

        public void Enable(bool value)
        {
            if (Enabled == value)
                return;

            Enabled = value && animator && animator.runtimeAnimatorController;
            
            if (!Enabled && animator)
                animator.StopPlayback();
        }   
        
        public void Attack(int type)
        {
            if (!Enabled)
                return;

            animator.SetFloat(ActionType, type);
            animator.SetTrigger(ActionTrigger);
        }

        public void Move(Vector3 dir)
        {
            if (!Enabled)
                return;

            animator.SetFloat(VelocityX, dir.x);
            animator.SetFloat(VelocityY, dir.z);
        }
    }
}