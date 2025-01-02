using UnityEngine;
using UnityEngine.UI;

namespace GalaxyGridiron
{
    public class AimMenu : MonoBehaviour
    {
        public Image windImage;
        public Sprite windSpriteUp;
        public Sprite windSpriteLeft;
        public Sprite windSpriteRight;


        [SerializeField] Animator _windAnim;


        private void Start()
        {
            //_windAnim = windImage.GetComponent<Animator>();
        }


        public void SetWindDirection(Vector2 windDirection)
        {
            Debug.Log($"WindAnimator is null, {_windAnim == null}, windDirection coming is {windDirection}");

            if (windDirection == Vector2.right) _windAnim.CrossFade("Right", .1f);
            else _windAnim.CrossFade("Left", .1f);
        }
    }

}