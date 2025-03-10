using System;
using Nekki.Game.Entities.Units.Abstractions;
using Nekki.Game.Entities.Units.Utils;
using Nekki.Game.Stats.Data;
using Nekki.Game.Stats.Utils;
using R3;
using UnityEngine;
using UnityEngine.AI;

namespace Nekki.Game.Entities.Units
{
    public class UnitNavmeshMovement : IUnitMovement
    {
        private readonly IUnitViewModel viewModel;
        private readonly NavMeshAgent navMeshAgent;
        private readonly Transform navTransform;
        private Transform steeringTarget;
        private Vector3 destination;
        
        private IDisposable steering;
        private IDisposable disposables;

        #region Properties
        public Vector3 Position => navTransform.position;
        public Quaternion Rotation => navTransform.rotation;
        public bool Enabled => navMeshAgent != null && navMeshAgent.enabled;
        public bool IsStopped => !Enabled || navMeshAgent.isStopped;
        public float RemainingDistance => Enabled ? Vector3.Distance(Position, destination) : float.MaxValue;

        public float StopDistance
        {
            get => navMeshAgent.stoppingDistance;
            set => navMeshAgent.stoppingDistance = value;
        }

        public float Speed
        {
            get => navMeshAgent.speed;
            set => navMeshAgent.speed = value;
        }

        public float TurnSpeed
        {
            get => navMeshAgent.angularSpeed;
            set => navMeshAgent.angularSpeed = value;
        }

        public float Acceleration
        {
            get => navMeshAgent.acceleration;
            set => navMeshAgent.acceleration = value;
        }

        public float Height
        {
            get => navMeshAgent.height;
            set => navMeshAgent.height = value;
        }

        public float Radius
        {
            get => navMeshAgent.radius;
            set => navMeshAgent.radius = value;
        }

        public int Priority
        {
            get => navMeshAgent.avoidancePriority;
            set => navMeshAgent.avoidancePriority = value;
        }
#endregion

        public UnitNavmeshMovement(
            NavMeshAgent navMeshAgent,
            IUnitViewModel viewModel)
        {
            this.navMeshAgent = navMeshAgent;
            this.viewModel = viewModel;
            navTransform = navMeshAgent.transform;
        }

        public void Enable(bool value)
        {
            if (Enabled == value)
                return;

            if (value)
            {
                navMeshAgent.enabled = true;
                OnEnabled();
                Stop();
                return;
            }

            Stop();
            OnDisabled();
            navMeshAgent.enabled = false;
        }

        public void SetSteering(Transform target)
        {
            if (!Enabled)
                return;
            
            Stop();
            
            if (!target)
                return;
            
            steeringTarget = target;
            destination = Vector3.positiveInfinity;
            steering = Observable.Interval(TimeSpan.FromSeconds(1/60f*10)).Subscribe(_ =>
            {
                if (steeringTarget)
                    MoveAuto(steeringTarget.position);
            });
        }

        private NavMeshPath localTestPath;
        public void MoveAuto(Vector3 worldPoint)
        {
            if (!Enabled)
                return;
            
            destination = worldPoint;
            localTestPath ??= new NavMeshPath();
            if (!navMeshAgent.CalculatePath(worldPoint, localTestPath))
                Debug.LogError($"Path is not calculated : {localTestPath.status}");
            
            OnPathChanged(localTestPath);
        }

        public void MoveRelative(Vector3 worldPoint)
        {
            if (!Enabled)
                return;

            destination = worldPoint;
            navMeshAgent.Move(worldPoint - Position);
        }

        public void Move(Vector3 worldPoint)
        {
            destination = worldPoint;
            navTransform.position = worldPoint;
            AdjustPosition();
        }

        public void Stop()
        {
            steering?.Dispose();
            steering = null;
            
            if (!navMeshAgent.isOnNavMesh)
                return;

            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
        }

        private void OnPathChanged(NavMeshPath unitPath)
        {
            if (!Enabled || unitPath is not {status: NavMeshPathStatus.PathComplete})
            {
                Debug.Log($"OnPathChanged cancelled : Enabled:{Enabled}, Path is not available");
                return;
            }
            
            navMeshAgent.SetPath(unitPath);
            navMeshAgent.isStopped = false;
        }

        private void AdjustPosition()
        {
            if (NavMesh.SamplePosition(navTransform.position, out var closestHit, 500f, navMeshAgent.areaMask))
                navTransform.position = closestHit.position;
        }

        private void OnEnabled()
        {
            var config = viewModel.Config;
            var stats = viewModel.StatsCollection;

            navMeshAgent.autoRepath = false;
            navMeshAgent.agentTypeID = config.movementType.AsAgentId();
            navMeshAgent.areaMask = config.walkableAreas;
            destination = Vector3.positiveInfinity;
            
            using var eventBuilder = Disposable.CreateBuilder();
            eventBuilder.Add(stats.SubscribeStat(StatType.MoveSpeed, stat => Speed = stat.Current));
            eventBuilder.Add(stats.SubscribeStat(StatType.MoveTurnSpeed, stat => TurnSpeed = stat.Current));
            eventBuilder.Add(stats.SubscribeStat(StatType.MoveAcceleration, stat => Acceleration = stat.Current));
            eventBuilder.Add(stats.SubscribeStat(StatType.Heght, stat => Height = stat.Current));
            eventBuilder.Add(stats.SubscribeStat(StatType.Radius, stat => Radius = stat.Current));
            eventBuilder.Add(stats.SubscribeStat(StatType.Priority, stat => Priority = (int)stat.Current));
            disposables = eventBuilder.Build();
            
            AdjustPosition();
        }

        private void OnDisabled()
        {
            steering?.Dispose();
            steering = null;
            disposables?.Dispose();
            disposables = null;
        }
    }
}