using TMPro;
using UnityEngine;

namespace GalaxyGridiron
{
    public class TouchdownObject : MonoBehaviour
    {
        public TMP_Text touchdownText;

        public void SetTouchdownText(string text) => touchdownText.text = text;
        public string GetTouchdownText() => touchdownText.text;
    }

}