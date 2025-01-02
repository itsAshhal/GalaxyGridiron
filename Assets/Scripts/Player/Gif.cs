using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;

namespace GalaxyGridiron
{
    public class Gif : MonoBehaviour
    {
        [SerializeField] Sprite[] _gifSprites;
        [SerializeField] Image _imgComponent;
        [SerializeField] SpriteRenderer _spriteRen;
        [SerializeField] int _delayInMilliseconds = 100;

        [Tooltip("If true then this gif will keep on animating forever")]
        [SerializeField] bool _keepAnimatingForever = true;

        public void Start()
        {
            //_imgComponent = GetComponent<Image>();
            TryGetComponent<Image>(out var img);
            if (img != null) _imgComponent = img;
            TryGetComponent<SpriteRenderer>(out var sr);
            if (sr != null) this._spriteRen = sr;
            AnimateGif();
        }

        private void AnimateGif()
        {
            StartCoroutine(AnimateGifCoroutine());
        }

        IEnumerator AnimateGifCoroutine()
        {
            int index = 0;
            foreach (var sprite in _gifSprites)
            {
                if (_imgComponent != null) _imgComponent.sprite = sprite;
                if (_spriteRen != null) _spriteRen.sprite = sprite;
                yield return new WaitForSeconds((float)_delayInMilliseconds / 1000);
                index++;
            }

            if (_keepAnimatingForever) AnimateGif();
        }
    }

}