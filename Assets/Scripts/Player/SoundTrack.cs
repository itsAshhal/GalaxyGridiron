using UnityEngine;

namespace GalaxyGridiron
{
    public class SoundTrack : MonoBehaviour
    {

        private AudioSource _audioSource;

        private void Awake() { _audioSource = GetComponent<AudioSource>(); }

        public void PlaySoundTrack() { _audioSource.Play(); }
        public void StopSoundTrack() { _audioSource.Stop(); }

    }

}