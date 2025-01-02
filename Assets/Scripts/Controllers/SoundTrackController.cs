using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace GalaxyGridiron
{
    /// <summary>
    /// Controls the soundTrack system across the whole game
    /// </summary>
    public class SoundTrackController : MonoBehaviour
    {
        [SerializeField] List<SoundTrack> _soundTracks;

        // Singleton instance
        public static SoundTrackController Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject); // Ensure there's only one instance
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object and its children across scenes
        }

        private void Start()
        {
            // We need to save the default sound track if no key exists
            ChangeSoundTrack();
        }

        /// <summary>
        /// This method only gets called when the player wants to change the sound track
        /// </summary>
        public async void ChangeSoundTrack()
        {
            bool soundTrackKeyExists = PlayerPrefs.HasKey("SoundTrack");
            if (soundTrackKeyExists)
            {
                // Get the key and play the sound with that index saved
                int soundTrackIndex = PlayerPrefs.GetInt("SoundTrack");
                SoundTrack currentTrack = null;

                for (int i = 0; i < _soundTracks.Count; i++)
                {
                    if (i == soundTrackIndex)
                    {
                        currentTrack = _soundTracks[i];
                        break;
                    }
                }

                await Task.Delay(1000);

                var otherTracks = _soundTracks.Where(track => track != currentTrack).ToList();

                // turn off all other tracks first
                foreach (var track in _soundTracks) track.StopSoundTrack();

                int currentTrackIndex = UnityEngine.Random.Range(0, otherTracks.Count);
                _soundTracks[currentTrackIndex].PlaySoundTrack();
                PlayerPrefs.SetInt("SoundTrack", currentTrackIndex);
            }
            else
            {
                // The soundTrack index doesn't exist, so play anything randomly.
                // This usually happens the first time the user plays the game
                var trackIndex = UnityEngine.Random.Range(0, _soundTracks.Count);
                _soundTracks[trackIndex].PlaySoundTrack();

                // Save the index as well
                PlayerPrefs.SetInt("SoundTrack", trackIndex);
            }
        }
    }
}
