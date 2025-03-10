using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Nekki.Common.LifecycleService;
using Nekki.Common.SceneService;
using Nekki.Common.SceneService.Abstractions;
using Nekki.Common.UIService.Abstractions;
using Nekki.Game.Abilities.Data;
using Nekki.Game.Entities.Player.Data;
using Nekki.Game.Scenarios.Data;
using Nekki.Lobby.Identity;
using R3;
using UnityEngine;

namespace Nekki.Lobby.UI.GameModes
{
    public interface IGameModesViewModel
    {
        ReactiveProperty<GameModeData> Current { get; }
        IEnumerable<GameModeData> Datas { get; }
    }

    public class GameModesViewModel : IInitable, IDisposable, IGameModesViewModel
    {
        private readonly IUIService uiService;
        private readonly ISceneService sceneService;
        public ReactiveProperty<GameModeData> Current { get; }
        public IEnumerable<GameModeData> Datas { get; }


        public GameModesViewModel(
            IUIService uiService,
            ISceneService sceneService,
            IEnumerable<GameModeData> modes)
        {
            this.uiService = uiService;
            this.sceneService = sceneService;
            Datas = modes;
            Current = new ReactiveProperty<GameModeData>();
        }

        public void Initialize()
        {
            try
            {
                Current.Skip(1).Subscribe(OnModeSelected);
                uiService.Begin<GameModesWindow>()
                    .WithInit(x => x.Bind(this))
                    .Show();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void OnModeSelected(GameModeData data)
        {
            uiService.Begin<GameModesWindow>().Hide();
            
            //TODO For example
            UserIdentity.Redirections.Add(new ScenarioData
            {
                Id = data.Id == SceneId.Demo3D ? ScenarioId.Default3D : ScenarioId.Default2D,
                Level = 0
            });
            
            UserIdentity.Redirections.Add(new PlayerData
            {
                Id = data.Id == SceneId.Demo3D ? PlayerId.Mage3D : PlayerId.Mage2D, 
                Abilities = new List<AbilityData> 
                {
                    // If you want, you can buff an ability, just add the stat data and it will be merged with the base stat in the game
                    new() {Id = data.Id == SceneId.Demo3D ? AbilityId.BattleAbility0 : AbilityId.BattleAbility3},
                    new() {Id = data.Id == SceneId.Demo3D ? AbilityId.BattleAbility1 : AbilityId.BattleAbility4},
                    new() {Id = data.Id == SceneId.Demo3D ? AbilityId.BattleAbility2 : AbilityId.BattleAbility5},
                }
            });
            //TODO For example
            
            sceneService.LoadAsync(data.Id).Forget();
        }

        public void Dispose()
        {
            Current?.Dispose();
        }
    }
}