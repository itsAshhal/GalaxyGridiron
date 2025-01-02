using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Data;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using Unity.VisualScripting;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Collections;
using static UnityEngine.EventSystems.EventTrigger;

namespace GalaxyGridiron
{
    /// <summary>
    /// Controls all the stats for the game
    /// </summary>
    public class StatsController : MonoBehaviour
    {
        public static StatsController Instance;

        private void Awake() { Instance = this; }

        #region Properties
        [SerializeField] TMP_Text _timeText;
        [SerializeField] TMP_Text _downText;
        [SerializeField] TMP_Text _toGoText;
        [SerializeField] TMP_Text _ballOnText;

        [SerializeField] TMP_Text _eliteText;  // our team's score
        [SerializeField] TMP_Text _awayText;  // opponent's team's score
        #endregion

        private void Start()
        {
            // subscribe the method to GameController's roundEvents;
            try { GameController.Instance.roundEndEvent += UpdateScoreOnRoundEnd; }
            catch (Exception e) { Debug.LogError($"Exception => {e.Message}"); }
        }

        public void ResetStats()
        {
            // reset stats to their default values
            _timeText.text = "15";
            _downText.text = "1";
            _toGoText.text = "10";
            _ballOnText.text = "50";
        }


        public void ResetBallon()
        {
            _ballOnText.text = "50";
        }


        /// <summary>
        /// so you can get the stats all at once with a sequence of course
        /// </summary>
        /// <returns></returns>
        public (int, int, int, int, int) GetAllStatsInNumbers()
        {
            return (int.Parse(_timeText.text), int.Parse(_downText.text), int.Parse(_toGoText.text), int.Parse(_ballOnText.text), 0);
        }

        public int GetTime() => int.Parse(_timeText.text);
        public int GetDown() => int.Parse(_downText.text);
        public int GetToGo() => int.Parse(_toGoText.text);
        public int GetBallOn() => int.Parse(_ballOnText.text);
        public int GetElite() => int.Parse(_eliteText.text);
        public int GetAway() => int.Parse(_awayText.text);



        public void UpdateAwayScore(int score)
        {
            // we need to add score not just the single update
            int previousScore = int.Parse(_awayText.text);
            int updatedScore = previousScore + score;
            //_awayText.text = score.ToString();
            _awayText.text = updatedScore.ToString();
        }
        public void UpdateElitScore(int score)
        {

            int previousScore = int.Parse(_eliteText.text);
            int updatedScore = previousScore + score;
            _eliteText.text = updatedScore.ToString();

            //_eliteText.text = score.ToString();
        }


        public void UpdateScoreOnRoundEnd(RoundEndData roundEndData)
        {

            // now only add a listener if the time unity is <= 0
            if (roundEndData.time <= 0)
            {
                GameUIController.Instance.QuestionImage.gameObject.SetActive(false);

                var btn = GameUIController.Instance.startNextRoundBtn;
                Debug.Log($"Button name is {btn.gameObject.name}");
                btn.onClick.RemoveAllListeners();

                var box = GameController.Instance.GetAnswerBox();
                /*box.SetInfo(new UserResult
                {
                    description = "",
                    heading = "Amazing, you beat 'em",
                    result = "",
                    yardsGained = "",
                    yardsToFirstDown = ""
                });
                ;*/

                // now add the final listener to show the user winning and losing results

                GameUIController.Instance.SetStartNextRoundButton(false);
                GameController.Instance.GetAnswerBox().CloseBox();

                bool isWinning = int.Parse(_eliteText.text) > int.Parse(_awayText.text);

                if (int.Parse(_eliteText.text) == int.Parse(_awayText.text)) isWinning = false;

                if (isWinning)
                {
                    Debug.Log($"EndState, {isWinning}");
                    GameUIController.Instance.SetEndScreen(EndScreenType.winning);
                    //var box = GameController.Instance.GetAnswerBox();
                    box.gameObject.SetActive(false);
                    box.gameObject.SetActive(true);
                    box.OpenBox();
                    box.SetInfo(new UserResult
                    {
                        description = "",
                        heading = "Amazing, you beat 'em",
                        result = "",
                        yardsGained = "",
                        yardsToFirstDown = ""
                    });
                }
                else
                {
                    Debug.Log($"EndState, {isWinning}");

                    GameUIController.Instance.SetEndScreen(EndScreenType.losing);

                    //var box = GameController.Instance.GetAnswerBox();
                    box.gameObject.SetActive(false);
                    box.gameObject.SetActive(true);
                    box.OpenBox();
                    box.SetInfo(new UserResult
                    {
                        description = "",
                        heading = "You'll get em next time!",
                        result = "",
                        yardsGained = "",
                        yardsToFirstDown = ""
                    });
                }


                //var btn = GameUIController.Instance.startNextRoundBtn;
                btn.gameObject.SetActive(false);
                btn.gameObject.SetActive(true);
                GameUIController.Instance.SetStartNextRoundButton(true);

                // now remove all listeners again and finally we need to show the user the end screen where there's survey
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() =>
                {
                    StartCoroutine(ButtonCoroutine());
                    IEnumerator ButtonCoroutine()
                    {
                        int correct = GameController.Instance.questionsCorrect;
                        int incorrect = GameController.Instance.questionsIncorrect;

                        int totalQuestions = 15;

                        float successRate = GetSuccessRate(correct, totalQuestions);

                        var box = GameController.Instance.GetAnswerBox();
                        box.CloseBox();
                        //await Task.Delay(1000);
                        yield return new WaitForSeconds(1);

                        box.gameObject.SetActive(false);
                        box.gameObject.SetActive(true);
                        box.OpenBox();

                        string line1 = $"Passing Success Rate: {successRate}%";
                        string line2 = $"You have completed this game and can close this window, but why not see if you can beat your score?";

                        box.SetInfo(new UserResult
                        {
                            description = "",
                            heading = line1,
                            result = "",
                            yardsGained = $"{line2}",
                            yardsToFirstDown = ""
                        });

                        // finale
                        GameUIController.Instance.SetFinalButtonsState(true);
                        var surveyBtn = GameUIController.Instance.takeSurveyBtn;
                        var retryBtn = GameUIController.Instance.retryBtn;

                        retryBtn.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
                        //surveyBtn.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));

                        GameUIController.Instance.hikeBtn.gameObject.SetActive(false);
                        GameUIController.Instance.extraPointBtn.gameObject.SetActive(false);
                        GameUIController.Instance.startNextRoundBtn.gameObject.SetActive(false);

                    }

                });



                //
                return;

            }



            if (roundEndData.down == 4)  // put it to 1 for testing
            {
                // instead of showing the self-made screen, we need to do the same thing when the opponent gets the ball and score some point

                // update the downToGo text to 4 so the user can see for a while before it gets reset to 1
                _downText.text = "4";

                GameUIController.Instance.OnClick_NextPlay();

                // we need to turn off some things as well
                TouchdownController.Instance.directionContents.SetActive(false);
                TouchdownController.Instance.girlsContainer.SetActive(true);
                TouchdownController.Instance.touchdownGroundSprite.enabled = true;

                GameUIController.Instance.SetQuestionImage(false);
                var extraPointBtn = GameUIController.Instance.extraPointBtn;
                string defaultText = extraPointBtn.GetComponent<TouchdownObject>().GetTouchdownText();
                extraPointBtn.GetComponent<TouchdownObject>().SetTouchdownText("Next");

                // set a temporary btn event so we can change the text of this button back to where and what it was
                extraPointBtn.onClick.AddListener(() => TempFunction());

                void TempFunction()
                {
                    extraPointBtn.GetComponent<TouchdownObject>().SetTouchdownText(defaultText);
                    extraPointBtn.onClick.RemoveListener(TempFunction);

                    // also reset the downToGo as well
                    this._downText.text = "1";


                    // the text on the TouchDown image is getting wrong so ammend it before we leave it from here
                    GameUIController.Instance.touchDownImage.GetComponent<TouchdownObject>().SetTouchdownText("Touchdown");
                }



                return;
            }

            // check if the 'Ball on' score is <= 0
            if (roundEndData.ballOn <= 0)
            {
                _timeText.text = roundEndData.time.ToString();
                _ballOnText.text = roundEndData.ballOn.ToString();
                TouchdownController.Instance.StartTouchdown();
                return;
            }

            else
            {
                // now when the data comes down here, update them on the topUI
                _timeText.text = roundEndData.time.ToString();
                _downText.text = roundEndData.down.ToString();
                _toGoText.text = roundEndData.toGo.ToString();
                _ballOnText.text = roundEndData.ballOn.ToString();

                // now we need to display the answerBox
                var box = GameController.Instance.GetAnswerBox();
                box.OpenBox();

                // alright there is one thing that we need to do, when the answer is correct everything is ok
                // but when the answer is incorrect, we only need to show the incomplete in the menu and thats it

                if (roundEndData.isCorrect)
                {
                    box.SetInfo(
                    new UserResult
                    {
                        result = roundEndData.isCorrect ? "Correct" : "Incorrect",
                        heading = "Player Results:",
                        description = $"You made it to the {roundEndData.ballOn} yard line",
                        yardsGained = $"Yards Gained: {roundEndData.randomYards}",
                        yardsToFirstDown = $"Yards to First Down: {roundEndData.down}"
                    }
                );
                    var startBtn = GameUIController.Instance.startNextRoundBtn;
                    startBtn.onClick.RemoveAllListeners();
                    startBtn.onClick.AddListener(() => { GameUIController.Instance.OnStartNextRound(); });
                }
                else
                {
                    box.SetInfo(
                    new UserResult
                    {
                        result = "",
                        heading = "Incomplete!",
                        description = "",
                        yardsGained = "",
                        yardsToFirstDown = "",
                    }
                );
                }

                // add a listener to the extraPoint as well
                if (roundEndData.ballOn <= 0)
                {
                    GameUIController.Instance.startNextRoundBtn.onClick.AddListener(() =>
                {
                    GameUIController.Instance.SetExtraPointsBtnState(false, "");
                    GameController.Instance.GetAnswerBox().CloseBox();

                    GameUIController.Instance.startNextRoundBtn.onClick.RemoveAllListeners();
                });
                }

                // darken the background as well
                Debug.Log($"Setting to true");
                GameUIController.Instance.EnableBackdrop();
                GameUIController.Instance.SetStartNextRoundButton(true);

                // now based on the result, show respective screens
                var screenType = roundEndData.isCorrect ? EndScreenType.winning : EndScreenType.losing;
                GameUIController.Instance.SetEndScreen(screenType);


                // shut down temporarily the questionImage as well
                GameUIController.Instance.QuestionImage.GetComponent<Animator>().CrossFade("Disappear", .1f);

            }



            // alright there's one last thing at the end of when we display the stat's based result to the user

        }

        /// <summary>
        /// Calculates the success rate of correct answers as a percentage.
        /// </summary>
        /// <param name="correct">Number of correct answers.</param>
        /// <param name="totalQuestions">Total number of questions.</param>
        /// <returns>Success rate as a percentage.</returns>
        public float GetSuccessRate(int correct, int totalQuestions)
        {
            if (totalQuestions == 0) return 0; // Avoid division by zero

            return (float)correct / totalQuestions * 100;
        }



    }


    /// <summary>
    /// When the round ends, either with correct or incorrect result
    /// we need to send this data over to implement stats
    /// </summary>
    public class RoundEndData
    {
        public int time;
        public int down;
        public int toGo;
        public int ballOn;
        public bool isCorrect;
        public int randomYards;

        public RoundEndData(int time, int down, int toGo, int ballOn, bool isCorrect, int randomYards)
        {
            this.time = time;
            this.down = down;
            this.toGo = toGo;
            this.ballOn = ballOn;
            this.isCorrect = isCorrect;
            this.randomYards = randomYards;
        }
    }

}