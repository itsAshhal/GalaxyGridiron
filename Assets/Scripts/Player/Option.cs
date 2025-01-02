using TMPro;
using UnityEngine;

namespace GalaxyGridiron
{
    public class Option : MonoBehaviour
    {
        [SerializeField] TMP_Text _optionText;
        public string GetOption() => _optionText.text;
    }

}