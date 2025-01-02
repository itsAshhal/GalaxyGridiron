using System;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

namespace GalaxyGridiron
{
    public class Ball : MonoBehaviour
    {
        private Vector2 _defaultPosition;

        private void Start() { _defaultPosition = transform.position; }

        private void OnTriggerStay2D(Collider2D other)
        {
            other.gameObject.TryGetComponent<Option>(out var option);
            if (option == null) return;

            Debug.Log($"Got the option {option.GetOption()}");

            try
            {
                // make sure the ball only hits and register the right option when we release the input
                if (GameController.Instance.IsClickedOrTouched()) return;  // this is if we are still holding the input we can't register the correct hit option
                GameController.Instance.OnBallTouchWithOption(this, option);

                // soon as we hit the collider/trigger, reset the ball position as we're calling from a stay function and don't wanna mess things up
                ResetPosition();
            }
            catch (Exception e) { Debug.Log($"Exception => {e.Message}"); }
        }

        public void SetCollider(bool colliderState) => GetComponent<CircleCollider2D>().enabled = colliderState;
        public void ResetPosition() => transform.position = _defaultPosition;
    }
}
