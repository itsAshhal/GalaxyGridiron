using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace GalaxyGridiron
{
    /// <summary>
    /// Managing individual player
    /// </summary>
    public class Player : MonoBehaviour
    {
        [Tooltip("The place where the question options will appear (Single option as this is an individual player)")]
        [SerializeField] Transform _optionBox;
        [SerializeField] TMP_Text _optionText;


        public void DisplayOption(string option)
        {
            // animate the option box and then the text on it
            var anim = _optionBox.GetComponent<Animator>();
            anim.CrossFade("Appear", .1f);
            _optionText.text = option;

        }

        public void HideOption()
        {
            var anim = _optionBox.GetComponent<Animator>();
            anim.CrossFade("Disappear", .1f);
            _optionText.text = "";
        }
    }

}