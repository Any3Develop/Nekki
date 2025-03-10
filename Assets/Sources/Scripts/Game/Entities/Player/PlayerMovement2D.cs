﻿using System;
using Nekki.Common.Events;
using Nekki.Common.InputSystem.Abstractions;
using Nekki.Common.Utilities;
using Nekki.Game.Entities.Player.Abstractions;
using Nekki.Game.Entities.Player.Data;
using Nekki.Game.Scenarios.Data;
using Nekki.Game.Scenarios.Events;
using Nekki.Game.Stats.Data;
using Nekki.Game.Stats.Utils;
using R3;
using UnityEngine;

namespace Nekki.Game.Entities.Player
{
    public class PlayerMovement2D : IPlayerMovement
    {
        private readonly IInputAction moveAction;
        private readonly Rigidbody2D target;
        private readonly Transform rbTrans;
        private Vector2 moveDir;
        
        private IDisposable disposables;
        private bool isPaused;
        
        public IPlayerViewModel ViewModel { get; }
        public bool Enabled { get; private set; }
        public float Speed { get; private set; }
        public float Acceleration { get; private set; }
        public float DumpingFactor { get; private set; }
        public float TurnSpeed { get; private set; }

        public PlayerMovement2D(
            Rigidbody2D playerRb, 
            IPlayerViewModel viewModel,
            IInputController<PlayerActions> input, 
            Vector3 initialPosition)
        {
            ViewModel = viewModel;
            target = playerRb;
            moveAction = input.Get(PlayerActions.Move);
            
            rbTrans = target.transform;
            moveDir = rbTrans.up;
            target.position = initialPosition;
        }

        public void Enable(bool value)
        {
            if (Enabled == value)
                return;
            
            if (value)
            {
                using var eventBuilder = Disposable.CreateBuilder();
                var stats = ViewModel.StatsCollection;
                
               eventBuilder.Add(MessageBroker.Receive<SenarioChangedEvent>().Subscribe(OnScenarioChanged));
               eventBuilder.Add(stats.SubscribeStat(StatType.MoveSpeed, stat => Speed = stat.Current));
               eventBuilder.Add(stats.SubscribeStat(StatType.MoveAcceleration, stat => Acceleration = stat.Current));
               eventBuilder.Add(stats.SubscribeStat(StatType.MoveDumpingFactor, stat => DumpingFactor = stat.Current));
               eventBuilder.Add(stats.SubscribeStat(StatType.MoveTurnSpeed, stat => TurnSpeed = stat.Current));
               eventBuilder.Add(Observable.EveryUpdate(UnityFrameProvider.FixedUpdate).Subscribe(_ => FixedUpdate()));
                
                disposables = eventBuilder.Build();
                moveAction.Enable();
                Enabled = true;
                return;
            }

            Release();
        }

        private void FixedUpdate()
        {
            if (isPaused)
                return;
            
            var input = moveAction.ReadValue<Vector2>();
            
            if (input.sqrMagnitude > 0)
            {
                var rotationInput = -input.x; 
                var moveInput = input.y;
            
                moveDir = rbTrans.up * moveInput;
                target.AddForce(moveDir * Acceleration * target.mass, ForceMode2D.Force);
            
                target.rotation += rotationInput * TurnSpeed * Time.deltaTime;
            }
            else
            {
                target.linearVelocity *= DumpingFactor;
            }
            
            if (target.linearVelocity.magnitude > Speed) 
                target.linearVelocity = target.linearVelocity.normalized * Speed;
            
            ViewModel.Animator.Move(target.linearVelocity);
        }


        private void Release()
        {
            Enabled = false;
            moveAction?.Disable();
            disposables?.Dispose();
            disposables = null;
        }

        private void OnScenarioChanged(SenarioChangedEvent evData)
        {
            isPaused = evData.Current.AnyFlags(ScenarioState.Paused);
        }

        public void Dispose() => Release();
    }
}