﻿#if TEXTMESHPRO
using Nekki.Common.UIService.AudioSource.Handlers;
using Nekki.Common.UIService.Data;
using UnityEngine;

namespace Nekki.Common.UIService.AudioSource.Configs
{
    [CreateAssetMenu(fileName = nameof(UITMPDropDownAudioHandler), menuName = "UIService/Audio/" + nameof(UITMPDropDownAudioHandler))]
    public class UITMPDropDownAudioConfig : UIAudioBaseConfig
    {
        [SerializeField] private bool includeDisabled = true;
        [SerializeField] private bool includeInherited = true;
        [SerializeField] private UIAudioClipData selectAudio;
                
        public bool IncludeDisabled => includeDisabled;
        public bool IncludeInherited => includeInherited;
        public UIAudioClipData SelectAudio => selectAudio;
    }
}
#endif