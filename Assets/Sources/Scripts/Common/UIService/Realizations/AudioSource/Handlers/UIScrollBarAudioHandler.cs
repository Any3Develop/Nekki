﻿using System.Collections.Generic;
using System.Linq;
using Nekki.Common.UIService.AudioSource.Configs;
using UnityEngine;
using UnityEngine.UI;

namespace Nekki.Common.UIService.AudioSource.Handlers
{
    public class UIScrollBarAudioHandler  : UIAudioHandlerBase<UIScrollBarAudioConfig>
    {
        protected List<Scrollbar> ListenComponents = new();
        private float lastValue;
        
        protected override void OnInit() => GetComponents(ref ListenComponents);

        protected override void OnDisposed()
        {
            if (ListenComponents != null)
                OnDisabled();
            
            ListenComponents = null;
        }

        protected override void OnEnabled()
        {
            foreach (var component in ListenComponents.Where(component => component))
                component.onValueChanged.AddListener(OnValueChanged);
        }

        protected override void OnDisabled()
        {
            foreach (var component in ListenComponents.Where(component => component))
                component.onValueChanged.RemoveListener(OnValueChanged);
        }

        protected virtual void OnValueChanged(float value)
        {
            if (!Initialized || !Enabled)
                return;
            
            if (Mathf.Abs(lastValue - value) < Config.Gap) 
                return;
            
            PlayAudioClip(Config.ScrollAudio);
            lastValue = value;
        }

        protected virtual void GetComponents<T>(ref List<T> components)
        {
            components ??= new List<T>();
            if (Config.IncludeInherited)
            {
                components.AddRange(Window.Content.GetComponentsInChildren<T>(Config.IncludeDisabled));
                return;
            }

            var specifiedType = typeof(T);  // to ensure the components are not inherited.
            components.AddRange(Window.Content
	            .GetComponentsInChildren<T>(Config.IncludeDisabled)
	            .Where(x => x.GetType() == specifiedType));
        }
    }
}