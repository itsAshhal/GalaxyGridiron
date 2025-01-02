using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines.ExtrusionShapes;

namespace GalaxyGridiron
{
    /// <summary>
    /// Manages the playerBase for handling all the 4 players running towards the goal with options on
    /// their heads
    /// </summary>
    public class PlayerComposition : MonoBehaviour
    {
        [SerializeField] SpriteRenderer[] _players;
        [SerializeField] Transform[] _playerTargets;
        [SerializeField] AnimationCurve _playersAnimationCurve;

        [Tooltip("Desired duration for the players to reach their targeted positions")]
        [SerializeField] float _desiredDuration = 1.0f;
        public SpriteRenderer[] GetPlayers() => _players;
        [SerializeField] bool _canMove = false;
        [SerializeField] float _moveSpeed = 2.0f;
        [SerializeField] float _questionSpawningDelay = 1.0f;

        private List<Vector2> _defaultStartingPositions = new List<Vector2>();

        private void Start()
        {
            foreach (var player in _players) _defaultStartingPositions.Add(player.transform.position);
        }

        private void Update()
        {
            if (!_canMove) return;

            // keep moving the playerComposition upwareds with a specific speed
            transform.position += Vector3.up * Time.deltaTime * _moveSpeed;


        }


        /// <summary>
        /// Animates all 4 players to their respective targets
        /// </summary>
        /// 
        [ContextMenu("Animate Starting Positions")]
        public void AnimateStartingPositions()
        {
            // lets try unparenting these players
            //foreach(var player in _players) player.transform.SetParent(null);

            // make sure at start all the texts in all the options are empty
            foreach (var player in _players) player.GetComponent<Player>().HideOption();

            StartCoroutine(AnimateStartingPositionsCoroutine());
        }

        private IEnumerator AnimateStartingPositionsCoroutine()
        {
            // when the game usually starts, turn off the collider/trigger on the ball
            GameController.Instance.GetBall().SetCollider(false);

            for (int i = 0; i < _players.Length; i++)
            {
                float timePassed = 0.0f;
                var player = _players[i];
                Vector2 startingPos = player.transform.position;
                Vector2 targetPos = _playerTargets[i].position;

                while (timePassed < _desiredDuration)
                {
                    // calculate the overall percentage of the time passed
                    float per = timePassed / _desiredDuration;

                    // Evaluate using the animation curve
                    per = _playersAnimationCurve.Evaluate(per);

                    // we can lerp now to create a dynamic animation
                    player.transform.position = Vector2.Lerp(startingPos, targetPos, per);

                    // update the timePassed as well
                    timePassed += Time.deltaTime;
                    yield return null;
                }
            }

            // when all the playes have been spawned, we can now spawn the questions
            StartCoroutine(SpawnQuestions());
        }

        private IEnumerator SpawnQuestions()
        {
            yield return new WaitForSeconds(_questionSpawningDelay);

            // load the question database from JSON
            //var questionsDatabase = GameData.Load<UserData>("UserData.json");

            // checking and testing built in data
            var questionsDatabase = QuestionData.GetSampleQuestions();

            Debug.Log($"IsNull {questionsDatabase == null}");

            // get a random question model
            Debug.Log($"Length of the questions is {questionsDatabase.questions.Length}");

            var randomQuestion = questionsDatabase.questions[Random.Range(0, questionsDatabase.questions.Length)];

            var options = randomQuestion.options;
            string question = randomQuestion.question;

            // update the UI as well
            GameUIController.Instance.SetQuestionText(question);
            GameController.Instance.SetCorrectAnswer(options[randomQuestion.correctOptionIndex]);

            for (int i = 0; i < _players.Length; i++)
            {
                var playerComponent = _players[i].GetComponent<Player>();
                playerComponent.DisplayOption(options[i]);

                Debug.Log($"Option is {options[i]}");
            }

            // animate the questionImage as well
            var img = GameUIController.Instance.QuestionImage;
            img.GetComponent<Animator>().CrossFade("Appear", .1f);

            // turn off the collider on the ball as well
            GameController.Instance.GetBall().SetCollider(true);
        }

        public void ResetPositions()
        {
            for (int i = 0; i < _players.Length; i++)
            {
                _players[i].transform.position = _defaultStartingPositions[i];
            }
        }

    }

}