﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Nekki.Lobby.UI.GameModes
{
    public class ModeItemView : MonoBehaviour
    {
        [field: SerializeField] public Button Button { get; private set; }
        [field: SerializeField] public TextMeshProUGUI Name { get; private set; }
    }
}