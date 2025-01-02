using UnityEngine;

namespace GalaxyGridiron
{
    public class Background : MonoBehaviour
    {
        private bool _isMoving = false;
        [SerializeField] float _scalingFactor = .05f;


        [Tooltip("Talking about the seconds duration here")]
        [SerializeField] float _howOftenShouldTheSpriteScale = 1.0f;

        private void Start()
        {
            // we need to keep scaling up the background to make it feel like tiled and infinite
            InvokeRepeating(nameof(KeepScalingUp), _howOftenShouldTheSpriteScale, _howOftenShouldTheSpriteScale);
        }

        private void KeepScalingUp()
        {
            // Increase the size of the sprite renderer
            SpriteRenderer spriteRen = GetComponent<SpriteRenderer>();
            spriteRen.size = new Vector2(spriteRen.size.x, spriteRen.size.y + _scalingFactor);

            // Adjust the position to anchor the bottom edge
            float positionOffset = _scalingFactor / 2f; // Half the scaling factor to account for center scaling
            transform.position = new Vector3(transform.position.x, transform.position.y + positionOffset, transform.position.z);
        }

        public void StartMovement()
        {
            _isMoving = true;
        }

        public void StopMovement()
        {
            _isMoving = false;
        }

        private void Update()
        {
            if (!_isMoving) return;

            // we need to move the background downwards to give it a sense of movement for the 
            // american footballers
            transform.position += Vector3.down * Time.deltaTime;
        }
    }
}