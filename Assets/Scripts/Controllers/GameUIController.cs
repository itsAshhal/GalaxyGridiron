using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements.Experimental;


namespace GalaxyGridiron
{
    public enum EndScreenType { winning, losing, nothing }
    public class GameUIController : MonoBehaviour
    {
        #region Properties
        public TMP_Text questionText;
        public Image QuestionImage;
        public Image darkenBackgroundImage;
        public Button startNextRoundBtn;

        public Image winningScreen;
        public Image losingScreen;

        // Touchdown
        public Image touchDownImage;
        public Button extraPointBtn;
        public AimMenu aimMenu;

        public Vector2 requiredDirection = Vector2.zero;

        public TouchdownObject opponentMsg;
        public Button hikeBtn;

        public Button takeSurveyBtn;
        public Button retryBtn;

        #endregion

        public static GameUIController Instance;

        private void Awake()
        {
            Instance = this;
            DisableBackdrop();
            SetEndScreen(EndScreenType.nothing);
            SetAimMenu(false);
            SetOpponentMessage(false, "");
            SetFinalButtonsState(false);

            // set some button actions
            startNextRoundBtn.onClick.AddListener(OnStartNextRound);
            extraPointBtn.onClick.AddListener(OnClick_ExtraPoint);
            hikeBtn.onClick.AddListener(OnClick_HikeBtn);
        }

        #region Method


        public void SetFinalButtonsState(bool value)
        {
            takeSurveyBtn.gameObject.SetActive(value);
            retryBtn.gameObject.SetActive(value);
        }

        public void SetTouchdownState(bool value, string touchdownText = "")
        {
            if (value)
            {
                extraPointBtn.GetComponent<Animator>().CrossFade("Appear", .1f);
                touchDownImage.GetComponent<Animator>().CrossFade("Appear", .1f);

                if (touchdownText != "") touchDownImage.GetComponent<TouchdownObject>().SetTouchdownText(touchdownText);
            }
            else
            {
                extraPointBtn.GetComponent<Animator>().CrossFade("Default", .1f);
                touchDownImage.GetComponent<Animator>().CrossFade("Default", .1f);
            }
        }

        public void SetOpponentMessage(bool state, string msg)
        {
            opponentMsg.gameObject.SetActive(state);
            opponentMsg.SetTouchdownText(msg);
        }

        public void SetHikeBtnState(bool value)
        {
            hikeBtn.gameObject.SetActive(value);
        }
        public void SetExtraPointsBtnState(bool value, string btnText = "")
        {
            extraPointBtn.gameObject.SetActive(value);
            extraPointBtn.GetComponent<Animator>().CrossFade("Appear", .1f);
            if (btnText != "") extraPointBtn.GetComponent<TouchdownObject>().SetTouchdownText(btnText);
            return;
            string name = value ? "Appear" : "Default";
            extraPointBtn.GetComponent<Animator>().CrossFade($"{name}", .1f);

        }
        public void OnClick_ExtraPoint()
        {
            TouchdownController.Instance.extraPointEvent?.Invoke();
        }


        /// <summary>
        /// This next play refers to when we're done shooting and its time for the opponent so shoot and score some points
        /// </summary>
        public void OnClick_NextPlay()
        {
            SetTouchdownState(true, "Opponent gets the ball!");
            int randomScore = Random.Range(1, 11);
            SetOpponentMessage(true, $"Opponent scored +{randomScore} points for the Away Team");

            // now remove this listener from the button
            extraPointBtn.onClick.RemoveAllListeners();

            // udpate the away score as well
            int awayScore = 0;
            bool isLastFiftyPercent = StatsController.Instance.GetTime() >= 7;
            // if (TouchdownController.Instance.isShootingDoneInRightDirection) awayScore = StatsController.Instance.GetElite() - 1;
            // else awayScore = StatsController.Instance.GetElite() + 1;

            awayScore = isLastFiftyPercent ? 7 : 0;

            // update the opponent message as well
            SetOpponentMessage(true, $"Opponent scored +{awayScore} points for the Away Team");

            // now add another listener to the extraPointBtn to restart the gameplay
            extraPointBtn.onClick.AddListener(() =>
            {
                SetTouchdownState(false);
                SetOpponentMessage(false, "");
                extraPointBtn.onClick.RemoveAllListeners();
                extraPointBtn.onClick.AddListener(OnClick_ExtraPoint);

                SetHikeBtnState(true);

                TouchdownController.Instance.touchdownGroundSprite.enabled = false;
                TouchdownController.Instance.girlsContainer.SetActive(false);

                // reset positions as well
                GameController.Instance.GetPlayerComposition().ResetPositions();
                var ball = GameController.Instance.GetBall();
                ball.gameObject.SetActive(false);
                ball.ResetPosition();

                // reset the stats time as well
                StatsController.Instance.ResetBallon();
            });

            StatsController.Instance.UpdateAwayScore(awayScore);

        }

        public void OnClick_HikeBtn()
        {
            GameController.Instance.GetPlayerComposition().AnimateStartingPositions();
            GameController.Instance.GetBall().gameObject.SetActive(true);
            SetHikeBtnState(false);
        }

        public void SetQuestionText(string question)
        {
            questionText.text = question;

            Debug.Log($"Text coming is {question}, isNull -> {questionText == null}");
        }

        public void SetAimMenu(bool value)
        {
            aimMenu.gameObject.SetActive(value);

            // now if its enabled select a random direction to show the user
            if (value)
            {
                int number = Random.Range(1, 4);
                /*                if (number == 1)
                                {
                                    requiredDirection = Vector2.up;
                                }*/
                if (number % 2 == 0)
                {
                    requiredDirection = Vector2.right;
                }
                else
                {
                    requiredDirection = -Vector2.right;
                }

                aimMenu.SetWindDirection(requiredDirection);
            }
        }

        public void OnStartNextRound()
        {
            Debug.Log($"Next round is called");
            GameController.Instance.StartNextRound();
            SetEndScreen(EndScreenType.nothing);


        }

        public void SetQuestionImage(bool value)
        {
            string name = value ? "Appear" : "Default";
            QuestionImage.GetComponent<Animator>().CrossFade($"{name}", .1f);
        }

        /// <summary>
        /// The dark kinda effect to make the front UI look more appealing
        /// </summary>
        public void EnableBackdrop()
        {
            darkenBackgroundImage.enabled = true;
        }
        public void DisableBackdrop()
        {
            darkenBackgroundImage.enabled = false;
        }

        public void SetStartNextRoundButton(bool value)
        {
            if (value) startNextRoundBtn.GetComponent<Animator>().CrossFade("Appear", .1f);
            else startNextRoundBtn.GetComponent<Animator>().CrossFade("Default", .1f);
        }

        public void SetEndScreen(EndScreenType endScreenType)
        {
            // enable and disable screens first
            if (endScreenType == EndScreenType.winning) { winningScreen.enabled = true; losingScreen.enabled = false; }
            else if (endScreenType == EndScreenType.losing) winningScreen.enabled = false; losingScreen.enabled = true;

            if (endScreenType == EndScreenType.winning)
            {
                // winning has the animator with it so use that
                Debug.Log($"Winning screen called");
                // var anim = winningScreen.GetComponent<Animator>();
                // anim.enabled = false;
                // anim.enabled = true;
                // anim.CrossFade("Appear", .1f);
                winningScreen.gameObject.SetActive(false);
                winningScreen.gameObject.SetActive(true);
            }
            else if (endScreenType == EndScreenType.losing)
            {

            }
            else { winningScreen.enabled = false; losingScreen.enabled = false; }
        }


        #endregion

    }

}