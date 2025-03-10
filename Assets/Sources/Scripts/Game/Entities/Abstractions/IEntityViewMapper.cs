using UnityEngine;

namespace Nekki.Game.Entities.Abstractions
{
    public interface IEntityViewMapper
    {
        TComponent Get<TComponent>();
        TComponent[] GetAll<TComponent>(bool recursive = false);
        bool TryGet<TComponent>(out TComponent result);
        
        TMap Map<TMap>(string id) where TMap : Component;
        TMap Map<TMap>() where TMap : Component;
        bool TryMap<TMap>(string id, out TMap result) where TMap : Component;
    }
}