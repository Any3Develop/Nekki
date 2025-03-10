using DG.Tweening;
using Nekki.Common.Pools;
using Nekki.Game.Entities.Abstractions;
using Nekki.Game.Entities.Units.Abstractions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Nekki.Game.Entities.Units
{
    [RequireComponent(typeof(IEntityViewMapper))]
    public class UnitView : PoolableView, IUnitView
    {
        public IEntityViewMapper Mapper { get; private set; }
        public IRuntimeEntity Entity => ViewModel;
        public IUnitViewModel ViewModel { get; private set; }
        [SerializeField] private bool initColor;
        private Transform root;

        private Vector3 sourceScale;
        private Tween shake;
        
        public void Shake()
        {
            shake?.Kill();
            root.localScale = sourceScale;
            shake = root.DOShakeScale(0.3f).SetAutoKill(true);
        }

        private void Awake()
        {
            Mapper = GetComponent<IEntityViewMapper>();
            root = Mapper.Map<Transform>("Root");
            sourceScale = root.localScale;
        }

        public void Init(IUnitViewModel viewModel)
        {
            if (DisposedLog())
                return;

            ViewModel = viewModel;
        }

        protected override void OnSpawned()
        {
            if (initColor) 
                return;
            
            initColor = true;
            var newColor = Color.Lerp(Random.value > 0.5f ? Color.red : Color.cyan,
                Random.value > 0.5f ? Color.green : Color.yellow, Random.value);

            foreach (var renderers in GetComponentsInChildren<Renderer>())
                renderers.material.color = newColor;
        }

        protected override void OnReleased()
        {
            Root.SetParent(null);
            ViewModel = null;
        }
    }
}