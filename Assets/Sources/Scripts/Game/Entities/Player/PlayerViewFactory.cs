using Nekki.Common.DependencyInjection;
using Nekki.Game.Entities.Player.Abstractions;
using UnityEngine;

namespace Nekki.Game.Entities.Player
{
    public class PlayerViewFactory : IPlayerViewFactory
    {
        private readonly IAbstractFactory abstractFactory;

        public PlayerViewFactory(IAbstractFactory abstractFactory)
        {
            this.abstractFactory = abstractFactory;
        }

        public IPlayerView Create(IPlayerViewModel viewModel, Transform root)
        {
            var view = abstractFactory.CreateUnityObject<PlayerView>(viewModel.Config.viewPrefab);
            view.Init(viewModel);
            
            var viewRoot = view.Mapper.Map<Transform>("Root");
            viewRoot.SetParent(root);
            viewRoot.localPosition = Vector3.zero;
            viewRoot.localRotation = Quaternion.identity;
            
            return view;
        }
    }
}