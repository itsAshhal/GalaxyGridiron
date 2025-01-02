using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GalaxyGridiron
{
    /// <summary>
    /// Manages all the fonts from single place, makes it hella easier
    /// </summary>
    public class FontController : MonoBehaviour
    {
        public static FontController Instance;
        [SerializeField] TMP_FontAsset _gameFont;

        public List<TMP_Text> sceneFonts; 

        private void Awake() { Instance = this; }

        private void Start()
        {
            ApplyFontSettings();
        }

        [ContextMenu("Apply Font Settings")]
        public void ApplyFontSettings()
        {
            sceneFonts = new List<TMP_Text>();

            // apply the font settings here
            var allTexts = FindObjectsByType<TMP_Text>(FindObjectsSortMode.None);
            Debug.Log($"Total fonts in the scene are {allTexts.Length}");

            // now apply the font asset to all the fonts
            foreach (var text in allTexts) 
            {
                text.font = _gameFont;
                sceneFonts.Add(text);
            }
            
        }
    }
}
