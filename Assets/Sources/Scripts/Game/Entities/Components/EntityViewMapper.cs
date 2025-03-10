using System;
using System.Linq;
using Nekki.Common.SerializableDictionary;
using Nekki.Game.Entities.Abstractions;
using UnityEngine;

namespace Nekki.Game.Entities.Components
{
    public class EntityViewMapper : MonoBehaviour, IEntityViewMapper
    {
        [Serializable]
        private class ComponentMap : SerializableDictionary<string, Component> {}
        
        [SerializeField] private GameObject container;
        [SerializeField] private ComponentMap componentMap = new();

        public TComponent Get<TComponent>() => container.GetComponent<TComponent>();
        public TComponent[] GetAll<TComponent>(bool recursive = false) 
            => recursive ? container.GetComponentsInChildren<TComponent>() : container.GetComponents<TComponent>();

        public bool TryGet<TComponent>(out TComponent result) => container.TryGetComponent(out result);
        
        public TMap Map<TMap>(string id) where TMap : Component
        {
            if (string.IsNullOrEmpty(id))
                throw new NullReferenceException("String id is null or empty");

            if (!componentMap.TryGetValue(id, out var result))
                throw new Exception($"{typeof(TMap).Name} doesn't mapped with id : {id}");

            return (TMap)result;
        }

        public TMap Map<TMap>() where TMap : Component => (TMap)componentMap.Values.FirstOrDefault(x => x is TMap);
        public bool TryMap<TMap>(string id, out TMap result)  where TMap : Component 
        {
            if (string.IsNullOrEmpty(id))
                throw new NullReferenceException("String id is null or empty");
            
            if (!componentMap.TryGetValue(id, out var component))
            {
                result = default;
                Debug.LogError($"{typeof(TMap).Name} doesn't mapped with id : {id}");
                return false;
            }

            result = (TMap) component;
            return true;
        }

        private void OnValidate()
        {
            if (!container)
                container = gameObject;
        }

        private void Reset()
        {
            componentMap.Add("Root", transform);
        }
    }
}