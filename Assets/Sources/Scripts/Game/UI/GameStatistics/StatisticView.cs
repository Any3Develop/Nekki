using TMPro;
using UnityEngine;

namespace Nekki.Game.UI.GameStatistics
{
    public class StatisticView : MonoBehaviour
    {
        [field: SerializeField] public TextMeshProUGUI Label { get; private set; }
        [field: SerializeField] public GameObject Container { get; private set; }

        public void SetActive(bool value)
        {
            Container.SetActive(value);
        }
    }
}