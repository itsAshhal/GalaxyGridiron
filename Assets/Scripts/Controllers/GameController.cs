using System;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;

namespace GalaxyGridiron
{
    /// <summary>
    /// Since there is no main player to control, we're gonna manage most of the things here in the GameController
    /// </summary>
    public class GameController : MonoBehaviour
    {
        public static GameController Instance;

        public CinemachineCamera cinemachineCamera;
        public InputActionPhase currentTouchPhase;

        [SerializeField] Ball _ball;
        public Ball GetBall() => this._ball;
        public bool inputEnabled = true;
        [SerializeField] AnswerBox answerBox;
        public AnswerBox GetAnswerBox() => this.answerBox;
        [SerializeField] int maxRandomizedYard = 15;
        [SerializeField] int minRandomizedYard = 1;

        // Just for testing
        public string correctAnswer = string.Empty;
        [SerializeField] Background _background;
        [SerializeField] PlayerComposition _playerComposition;
        public PlayerComposition GetPlayerComposition() => _playerComposition;
        public Action<RoundEndData> roundEndEvent;

        public int questionsCorrect = 0;
        public int questionsIncorrect = 0;

        private bool isPressed = false;
        public bool IsClickedOrTouched() => isPressed;


        private void Awake()
        {
            Instance = this;


        }

        private void Start()
        {
            // bind the callbacks for touchHold events
            try
            {
                // main callback event for touchControls
                InputController.Instance.touchHoldEvent += OnTouchHold;

                // check and test the click event
                InputController.Instance.clickEvent += ctx =>
                {
                    isPressed = ctx.ReadValueAsButton();
                    //Debug.Log($"Getting the screenPosition => {ctx.ReadValue<Vector2>()}");
                };
            }
            catch (Exception e)
            {
                Debug.LogError($"Exception => {e.Message}");
            }

            // start the background movement as well
            _background.StartMovement();

            // lets and start the game as well
            //_playerComposition.AnimateStartingPositions();

            // hide the ball at start
            _ball.gameObject.SetActive(false);

            // reset stats when the game starts for the first time
            StatsController.Instance.ResetStats();
        }

        private void OnTouchHold(InputAction.CallbackContext callback)
        {
            if (inputEnabled == false) return;

            currentTouchPhase = callback.phase;

            // get the screen position
            Vector2 screenPos = callback.ReadValue<Vector2>();

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(screenPos), Vector2.zero);

            bool gotTheBall = false;
            gotTheBall = hit.collider != null;

            if (gotTheBall == false) return;

            // also we need to make sure if there is a click/touch as well
            if (isPressed == false) return;


            var worldPos = Accessories.GetWorldPosition(callback.ReadValue<Vector2>());

            if (callback.phase == InputActionPhase.Performed)
            {
                _ball.transform.position = worldPos;
            }
        }

        /// <summary>
        /// Will start next round when the one option that is selected is either correct or incorrect
        /// </summary>
        public void StartNextRound()
        {
            Debug.Log($"Starting next round");
            _ball.ResetPosition();
            _ball.SetCollider(true);
            _playerComposition.ResetPositions();

            // now disable some stuff and then re-animate the players
            GameUIController.Instance.DisableBackdrop();
            GameUIController.Instance.SetStartNextRoundButton(false);

            // re-animating
            _playerComposition.AnimateStartingPositions();

            // we need to shut down some animations
            GameUIController.Instance.QuestionImage.GetComponent<Animator>().CrossFade("Disappear", .1f);
            answerBox.CloseBox();
        }

        /// <summary>
        /// Will restart the game, either from this scene or from the into
        /// </summary>
        /// <param name="restartFromSplash">if TRUE then restart the whole game from splash screen</param>
        public void RestartWholeGame(bool restartFromSplash = false)
        {
            switch (restartFromSplash)
            {
                case true: SceneManager.LoadScene(0); break;
                case false: SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); break;
            }
        }

        public void OnBallTouchWithOption(Ball ball, Option selectedOption)
        {
            // soon as this function is called, we need to make sure the dragging/touchHold
            // doesn't work anymore so we can process the option and scoring stuff
            ball.GetComponent<CircleCollider2D>().enabled = false;

            // since the user has selected one of the options, we now need to show the answerBox
            //answerBox.OpenBox();

            // userResult and roundEndData
            // first calculate everything and then update the UI accordingly
            int updatedTime = StatsController.Instance.GetTime() - 1;
            // int updatedTime = 0;
            int randomNumberOfYards = GetRandomNumberOfYards();
            int updatedBallOn = StatsController.Instance.GetBallOn() - randomNumberOfYards;
            // updating Down
            int updatedDown = StatsController.Instance.GetDown();
            if (randomNumberOfYards < StatsController.Instance.GetToGo()) updatedDown += 1;
            else updatedDown = 1;
            int updatedToGo = StatsController.Instance.GetToGo() - randomNumberOfYards;


            if (randomNumberOfYards >= StatsController.Instance.GetToGo()) updatedToGo = 10;

            // now update these stats so we can display them in the UI as well
            bool isAnswerCorrect = correctAnswer == selectedOption.GetOption();

            // check for either CORRECT or INCORRECT
            if (isAnswerCorrect)
            {
                // update stats to the StatsController
                roundEndEvent?.Invoke(
                    new RoundEndData(
                       updatedTime,
                       updatedDown,
                       updatedToGo,
                       updatedBallOn,
                       correctAnswer == selectedOption.GetOption(),
                       randomNumberOfYards
                    )
                );

                questionsCorrect++;
            }
            else
            {
                // update stats to the StatsController
                roundEndEvent?.Invoke(
                    new RoundEndData(
                       StatsController.Instance.GetTime() - 1,
                       StatsController.Instance.GetDown() + 1,
                       StatsController.Instance.GetToGo(),
                       StatsController.Instance.GetBallOn(),
                       //    51,
                       correctAnswer == selectedOption.GetOption(),
                       randomNumberOfYards

                    )
                );
                questionsIncorrect++;
            }

        }

        /// <summary>
        /// This method gets invoked when 4 options get assembled and we need to store the correct answer somewhere
        /// </summary>
        /// <param name="correctAnswer"></param>
        public void SetCorrectAnswer(string correctAnswer)
        {
            // we need to set the correct answer here
            this.correctAnswer = correctAnswer;
        }

        private int GetRandomNumberOfYards()
        {
            //return 25;
            return UnityEngine.Random.Range(minRandomizedYard, maxRandomizedYard);
        }

    }
}
