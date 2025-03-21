﻿using System.Collections;
using UnityEngine;

namespace Nekki.Common.UIService.AnimationSource
{
    public class UIAnimationCoroutineRunner : MonoBehaviour
    {
        private static MonoBehaviour instance;

        private static void SetupCrossSceneSingleton()
        {
            if (instance)
                return;

            instance = new GameObject(nameof(UIAnimationCoroutineRunner)).AddComponent<UIAnimationCoroutineRunner>();
            DontDestroyOnLoad(instance);
        }

        public new static Coroutine StartCoroutine(IEnumerator routine)
        {
            SetupCrossSceneSingleton();
            return instance.StartCoroutine(routine);
        }

        public new static void StopCoroutine(Coroutine coroutine)
        {
            if (coroutine == null)
                return;
            
            SetupCrossSceneSingleton();
            instance.StopCoroutine(coroutine);
        }
    }
}