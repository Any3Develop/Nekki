using Nekki.Game.Entities.Units.Data;
using UnityEngine;

namespace Nekki.Game.Entities.Units.Abstractions
{
    public interface IUnitViewModelFactory
    {
        IUnitViewModel Create(UnitConfig config, Vector3 position);
    }
}