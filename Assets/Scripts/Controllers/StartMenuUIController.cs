using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GalaxyGridiron
{
    public class StartMenuUIController : MonoBehaviour
    {
        [SerializeField] TMP_Text _motivationText;
        [SerializeField] int _delayInMilliseconds = 100;
        [SerializeField] Image _backgroundImage;
        [SerializeField] float _darkAmount = .5f;
        [SerializeField] float _duration = 1.0f;
        /// <summary>
        /// When the motivation texts fully appears, this event gets invoked
        /// </summary>
        [SerializeField] UnityEvent EventAfterTextsAppears;



        private void Start()
        {
            StartMotivation();

            // subscribe to the darken image method as well
            EventAfterTextsAppears.AddListener(() => { DarkenImage(_backgroundImage, _darkAmount, _duration); });
        }

        private void StartMotivation()
        {
            string text = _motivationText.text;
            _motivationText.text = string.Empty;

            foreach (var letter in text)
            {
                _motivationText.text += letter;
                //await Task.Delay(_delayInMilliseconds);
            }

            EventAfterTextsAppears?.Invoke();
        }

        // Method to darken an image
        public void DarkenImage(Image image, float darkenAmount, float duration)
        {
            if (image == null)
            {
                Debug.LogError("Image is null. Please assign a valid Image component.");
                return;
            }

            if (darkenAmount < 0 || darkenAmount > 1)
            {
                Debug.LogError("Darken amount must be between 0 (no darkening) and 1 (completely black).");
                return;
            }

            StartCoroutine(DarkenImageCoroutine(image, darkenAmount, duration));
        }

        // Coroutine to handle the darkening effect
        private IEnumerator DarkenImageCoroutine(Image image, float darkenAmount, float duration)
        {
            Color originalColor = image.color;
            Color targetColor = new Color(
                originalColor.r * (1 - darkenAmount),
                originalColor.g * (1 - darkenAmount),
                originalColor.b * (1 - darkenAmount),
                originalColor.a // Keep the alpha unchanged
            );

            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                image.color = Color.Lerp(originalColor, targetColor, elapsedTime / duration);
                yield return null;
            }

            // Ensure the final color is set precisely
            image.color = targetColor;
        }


    }

}