using Nekki.Game.Entities.Units.Abstractions;
using Nekki.Game.Stats.Abstractions;
using Nekki.Game.Stats.Data;
using Nekki.Game.Stats.Utils;
using R3;
using UnityEngine;

namespace Nekki.Game.Entities.Units
{
    [RequireComponent(typeof(IUnitView))]
    public class UnitViewFollower : MonoBehaviour, IUnitViewFollower
    {
        private CompositeDisposable disposables;
        private Transform selfCached;
        private Transform steering;
        
        public bool Enabled { get; private set; }
        public float SpeedDumping { get; private set; }
        public float TurnDumping { get; private set; }
        public float Altitude { get; private set; }
        public IUnitView View { get; private set; }

        private void Awake()
        {
            View = GetComponent<IUnitView>();
            selfCached = transform;
        }

        public void SetSteering(Transform value) => steering = value;

        public void Enable(bool value)
        {
            if (Enabled == value)
                return;

            Enabled = value;
            if (value)
            {
                disposables?.Dispose();
                disposables = new CompositeDisposable();
                
                var stats = View.ViewModel.StatsCollection;
                stats.SubscribeStat(StatType.FlyDumping, stat => SpeedDumping = CalcDumping(stat, StatType.MoveSpeed))
                    .AddTo(disposables);
                
                stats.SubscribeStat(StatType.FlyAltitude, stat => Altitude = stat.Current)
                    .AddTo(disposables);
                
                stats.SubscribeStat(StatType.FlyTrunDumping, stat => TurnDumping = CalcDumping(stat, StatType.MoveTurnSpeed))
                    .AddTo(disposables);
                return;
            }

            Release();
        }
        
        private float CalcDumping(IRuntimeStat dumpingStat, StatType baseType)
        {
            if (!Enabled || dumpingStat == null || View?.ViewModel == null || !View.ViewModel.StatsCollection.TryGet(baseType, out var baseStat))
                return 0;

            return baseStat.Current * dumpingStat.Current;
        }

        private void Update()
        {
            if (!Enabled)
                return;

            var targetPos = steering.position;
            targetPos.y = Altitude;
            var delta = Time.deltaTime;
            var targetRot = Quaternion.LookRotation(steering.forward);
            var currRot = selfCached.rotation;

            var maxAngleDifference = Quaternion.Angle(currRot, targetRot);
            var t = Mathf.Min(1f, TurnDumping * Time.deltaTime / maxAngleDifference);
            selfCached.position = Vector3.Lerp(selfCached.position, targetPos, SpeedDumping * delta);
            selfCached.rotation = Quaternion.Slerp(currRot, targetRot, t);
        }

        private void Release()
        {
            Enabled = false;
            disposables?.Dispose();
            disposables = null;
            steering = null;
        }

        private void OnDestroy()
        {
            Release();
            selfCached = null;
            View = null;
        }
    }
}