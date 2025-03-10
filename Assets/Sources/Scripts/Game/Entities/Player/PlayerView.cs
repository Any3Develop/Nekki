using DG.Tweening;
using Nekki.Game.Entities.Abstractions;
using Nekki.Game.Entities.Player.Abstractions;
using UnityEngine;

namespace Nekki.Game.Entities.Player
{
    [RequireComponent(typeof(IEntityViewMapper))]
    public class PlayerView : MonoBehaviour, IPlayerView
    {
        public IRuntimeEntity Entity => ViewModel;
        public IEntityViewMapper Mapper { get; private set; }
        public IPlayerViewModel ViewModel { get; private set; }
        private Transform root;
        private Vector3 sourceScale;
        private Tween shake;

        private void Awake()
        {
            Mapper = GetComponent<IEntityViewMapper>();
            root = Mapper.Map<Transform>("Root");
            sourceScale = root.localScale;
        }

        public void Shake()
        {
            shake?.Kill();
            root.localScale = sourceScale;
            shake = root.DOShakeScale(0.3f).SetAutoKill(true);
        }

        public void Init(IPlayerViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        private void OnDestroy()
        {
            ViewModel = null;
            Mapper = null;
        }
    }
}