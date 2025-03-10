using Nekki.Game.Entities.Units.Abstractions;
using UnityEngine;

namespace Nekki.Game.Entities.Units
{
    public class UnitAvationViewModel : UnitViewModel
    {
        private IUnitViewFollower viewFollower;

        protected override void OnInit(Vector3 position)
        {
            Movement.Move(position);
            View.Mapper.Map<Transform>("Root").position = position;
            View.Mapper.TryGet(out viewFollower);
            viewFollower.SetSteering(Root);
        }

        protected override void OnSpawned()
        {
            viewFollower.Enable(true);
            base.OnSpawned();
        }

        protected override void OnReleased()
        {
            viewFollower.Enable(false);
            viewFollower = null;
            base.OnReleased();
        }

        protected override void OnDisposed()
        {
            viewFollower = null;
            base.OnDisposed();
        }
    }
}