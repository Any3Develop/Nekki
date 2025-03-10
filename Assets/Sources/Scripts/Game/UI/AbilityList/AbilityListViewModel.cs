﻿using System;
using System.Collections.Generic;
using System.Linq;
using Nekki.Common.Events;
using Nekki.Common.InputSystem.Abstractions;
using Nekki.Common.LifecycleService;
using Nekki.Common.UIService.Abstractions;
using Nekki.Game.Abilities.Abstractions;
using Nekki.Game.Context.Abstractions;
using Nekki.Game.Entities.Player.Data;
using Nekki.Game.Scenarios.Data;
using Nekki.Game.Scenarios.Events;
using R3;
using UnityEngine;

namespace Nekki.Game.UI.AbilityList
{
    public interface IAbilityListViewModel
    {
        ReactiveProperty<IAbility> Current { get; }
        IEnumerable<IAbility> Datas { get; }
    }
    
    public class AbilityListViewModel : IInitable, IDisposable, IAbilityListViewModel
    {
        private readonly IUIService uiService;
        private readonly IGameContext gameContext;
        private readonly IInputAction shiftLeft;
        private readonly IInputAction shiftRight;
        private readonly IInputAction execute;
        private IDisposable executeHolding;
        
        public ReactiveProperty<IAbility> Current { get; }
        public IEnumerable<IAbility> Datas => gameContext.Player.AbilityCollection;


        public AbilityListViewModel(
            IUIService uiService,
            IGameContext gameContext,
            IInputController<PlayerActions> input)
        {
            this.uiService = uiService;
            this.gameContext = gameContext;
            shiftLeft = input.Get(PlayerActions.ShiftLeft);
            shiftRight = input.Get(PlayerActions.ShiftRight);
            execute = input.Get(PlayerActions.Execute);
            Current = new ReactiveProperty<IAbility>();
        }

        public void Initialize()
        {
            try
            {
                MessageBroker.Receive<SenarioChangedEvent>()
                    .Where(evData => evData.Current == ScenarioState.Playing)
                    .Take(1)
                    .Subscribe(_ => Start());
            
                MessageBroker.Receive<SenarioChangedEvent>()
                    .Where(evData => evData.Current == ScenarioState.Ended)
                    .Take(1)
                    .Subscribe(_ => End());
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void Dispose()
        {
            Current?.Dispose();
        }

        private void Start()
        {
            Current.OnNext(gameContext.Player.AbilityCollection.First());
            shiftLeft.OnPerformed += OnAbilityShiftLeft;
            shiftRight.OnPerformed += OnAbilityShiftRight;
            execute.OnPerformed += OnAbilityExecute;
            uiService.Begin<AbilityListWindow>()
                .WithInit(x => x.Bind(this))
                .Show();
        }

        private void End()
        {
            executeHolding?.Dispose();
            executeHolding = null;
            shiftLeft.OnPerformed -= OnAbilityShiftLeft;
            shiftRight.OnPerformed -= OnAbilityShiftRight;
            execute.OnPerformed -= OnAbilityExecute;
            uiService.Begin<AbilityListWindow>().Hide();
        }

        private void OnAbilityExecute(IInputContext context) // TODO for system tests
        {
            if (!context.Performed)
                return;
            
            if (executeHolding == null)
            {
                executeHolding = Observable.EveryUpdate().Subscribe(_ => Current.Value?.Execute());
                return;
            }
            
            executeHolding?.Dispose();
            executeHolding = null;
        }
        
        private void OnAbilityShiftLeft(IInputContext context)
        {
            var abilityCollection = gameContext.Player.AbilityCollection;
            var currentIndex = abilityCollection.IndexOf(Current.Value);
            currentIndex = (currentIndex - 1 + abilityCollection.Count) % abilityCollection.Count;
            Current.OnNext(abilityCollection[currentIndex]);
        }

        private void OnAbilityShiftRight(IInputContext context)
        {
            var abilityCollection = gameContext.Player.AbilityCollection;
            var currentIndex = abilityCollection.IndexOf(Current.Value);
            currentIndex = (currentIndex + 1) % abilityCollection.Count;
            Current.OnNext(abilityCollection[currentIndex]);
        }

    }
}