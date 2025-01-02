using System;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GalaxyGridiron
{
    /// <summary>
    /// Touchdown occrurs when the 'Ball on' score gets below 0.
    /// </summary>
    public class TouchdownController : MonoBehaviour
    {
        public static TouchdownController Instance;
        public GameObject girlsContainer;
        public SpriteRenderer touchdownGroundSprite;
        public Action extraPointEvent;

        [Tooltip("These are the contents which will be appeared after the user clicks on extra point")]
        public GameObject directionContents;

        /// <summary>
        /// The 3 direction buttons that we have to use for one to throw the ball in a certain direction
        /// </summary>
        private bool _canSelectDirection = false;

        [Tooltip("So it'll be easier for use to throw balls in this direction")]
        [SerializeField] Transform[] _directionTransforms;
        private string _directionName = "";
        [SerializeField] Transform _directionBall;
        [SerializeField] float _force = 5.0f;
        public Vector2 _savedDirection = Vector2.zero;

        [HideInInspector]
        public bool isShootingDoneInRightDirection = false;
        private Vector2 _ballDefaultPosition = Vector2.zero;

        private void Awake()
        {
            Instance = this;
            extraPointEvent += OnExtraPoint;
        }

        private void Start()
        {
            girlsContainer.SetActive(false);
            touchdownGroundSprite.enabled = false;
            directionContents.SetActive(false);
            _ballDefaultPosition = _directionBall.transform.position;





        }

        private void Update()
        {
            // bind the input function for raycasting 

            if (_canSelectDirection == false) return;



            // also check if the button is pressed or touched as well
            if (GameController.Instance.IsClickedOrTouched() == false) return;

            Debug.Log($"Inside the phase");


            // case a ray and check the position when it detects the direction button colliders
            //Vector2 screenPos = ctx.ReadValue<Vector2>();
            Vector2 mouseScreenPosition = InputController.Instance.mouseCurrentScreenPosition;
            Vector2 screenToWorldPos = Accessories.GetWorldPosition(mouseScreenPosition);

            Debug.Log($"Mouse position we're getting is {mouseScreenPosition}");
            Debug.Log($"World position we're getting is {screenToWorldPos}");

            RaycastHit2D hit = Physics2D.Raycast(screenToWorldPos, Vector2.zero, Mathf.Infinity);

            if (hit.collider != null)
            {


                Debug.Log($"hitCollider is {hit.collider.name}");
                _canSelectDirection = false;

                // now store the direction
                _directionName = hit.collider.name;

                _savedDirection = _directionName.Equals("up") ? Vector2.up : _directionName.Equals("left") ? Vector2.left : Vector2.right;

                // now throw the ball into that direction
                var directionTr = _directionTransforms.Where(dir => dir.gameObject.name == _directionName)?.First().transform;
                if (directionTr != null)
                {
                    _canSelectDirection = false;

                    //var ballRb = _directionBall.AddComponent<Rigidbody2D>();
                    //ballRb.AddForce(directionTr.up * _force, ForceMode2D.Impulse);

                    // Add rotation by setting angular velocity (in degrees per second)
                    //float rotationSpeed = 360f; // Adjust this value to control the rotation speed
                    //ballRb.angularVelocity = rotationSpeed;

                    // after shooting the ball, 2 things, either right direction according to the aim menu or the wrong direction
                    Invoke(nameof(AfterShootingTheBall), 1.5f);  // 1.5 seconds are more than enough for an average wait
                }
            }
            else Debug.Log($"not hitting anything");

        }


        private void AfterShootingTheBall()
        {
            // remove the rigidbody of the ball
            _directionBall.TryGetComponent<Rigidbody2D>(out var rb);
            if (rb != null) Destroy(rb);

            GameUIController.Instance.SetTouchdownState(false);
            Debug.Log($"Saved direction is {_savedDirection}, required direction is {GameUIController.Instance.requiredDirection}");
            bool hasMissed = false;
            if (_savedDirection == Vector2.right && GameUIController.Instance.requiredDirection == -Vector2.right) hasMissed = false;
            else if (_savedDirection == -Vector2.right && GameUIController.Instance.requiredDirection == Vector2.right) hasMissed = false;
            else hasMissed = true;


            // if the direction that we chose is incorrect than we need to set the ball to either left or right (not to the goal)
            float rotationSpeed = 360f;
            var ballRb = _directionBall.gameObject.AddComponent<Rigidbody2D>();
            if (hasMissed)
            {
                // shoot the ball either to the left or the right one
                var shootingDirection = UnityEngine.Random.Range(1, 5) % 2 == 0 ? Vector2.right : -Vector2.right;


                ballRb.AddForce(shootingDirection * _force, ForceMode2D.Impulse);

                ballRb.angularVelocity = rotationSpeed;
            }
            else
            {
                // we have shot the ball in the direction of course
                ballRb.AddForce(Vector2.up * _force, ForceMode2D.Impulse);
                ballRb.angularVelocity = rotationSpeed;
            }


            isShootingDoneInRightDirection = !hasMissed;
            string resultText = hasMissed ? "MISSED!" : "RIGHT ONE!";
            GameUIController.Instance.SetTouchdownState(true, $"{resultText}");
            GameUIController.Instance.SetExtraPointsBtnState(false);
            GameUIController.Instance.SetAimMenu(false);

            // now after a sec invoke another method
            Invoke(nameof(AfterShootingResult), 1f);
        }

        public void AfterShootingResult()
        {
            // now change the callbacks for the touchdown events
            var btn = GameUIController.Instance.extraPointBtn;
            btn.gameObject.SetActive(true);
            btn.GetComponent<Animator>().CrossFade("Appear", .1f);
            directionContents.SetActive(false);

            btn.GetComponent<TouchdownObject>().SetTouchdownText("Next Play");

            // chanhge the button event too
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(GameUIController.Instance.OnClick_NextPlay);

            // since we're showing the result, we need to add the elite and away scores(texts)
            // lets do a trick, if our player shoots in the right direction then his score will always be one greater than the opponent's


            // Our team's score is being updated soon when the Touchdown event gets started, right in the method StartTouchdown().

            // we got some points at the start of touch down, we can also get an extra point if we shoot correct
            if (isShootingDoneInRightDirection) StatsController.Instance.UpdateElitScore(1);
        }
        public void StartTouchdown()
        {
            StatsController.Instance.UpdateElitScore(UnityEngine.Random.Range(5, 10));

            GameUIController.Instance.SetTouchdownState(true);
            girlsContainer.SetActive(true);

            // we need to shut down some other things as well
            GameUIController.Instance.SetStartNextRoundButton(false);
            GameUIController.Instance.SetExtraPointsBtnState(true, "");
            GameUIController.Instance.SetQuestionImage(false);
            touchdownGroundSprite.enabled = true;
            GameController.Instance.GetAnswerBox().CloseBox();

            // remove any Rigidbody component if its on the ball already
            _directionBall.TryGetComponent<Rigidbody2D>(out var rb);
            if (rb != null) Destroy(rb);

            // set the proper position of the ball as well
            _directionBall.transform.position = _ballDefaultPosition;
            _directionBall.transform.rotation = Quaternion.identity;
        }

        private void OnExtraPoint()
        {
            // now we need to turn off the touchdownImage
            GameUIController.Instance.SetTouchdownState(false);

            // turn off the girls container as well
            girlsContainer.SetActive(false);

            // then we need to show the contents like ball and the direction sprites
            directionContents.SetActive(true);

            // enable the aim menu as well
            GameUIController.Instance.SetAimMenu(true);

            _canSelectDirection = true;
        }
        public float gizmosMaxDistance = 10.0f;
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * gizmosMaxDistance);
        }


    }

}