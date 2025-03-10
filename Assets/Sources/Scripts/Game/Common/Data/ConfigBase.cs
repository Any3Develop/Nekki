using System;
using UnityEngine;

namespace Nekki.Game.Common.Data
{
    public abstract class ConfigBase : ScriptableObject
    {
        [Header("General")]
        public string id;

#if UNITY_EDITOR
        protected virtual void Reset()
        {
            id = Guid.NewGuid().ToString();
        }

        protected virtual void OnValidate()
        {
            id = Guid.NewGuid().ToString();
        }
#endif
    }
}