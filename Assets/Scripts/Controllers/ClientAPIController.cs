using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace GalaxyGridiron
{
    public class ClientAPIController : MonoBehaviour
    {
        public static ClientAPIController Instance;

        private void Awake()
        {
            Instance = this;
        }

        [ContextMenu("Check API")]
        public void CheckAPI()
        {
            SendGameData(
            "eid",            // Pass the actual eid token here
            "TestGame",
            "8/10",
            80,
            "passed 80%"
            );
        }




        private const string BaseURL = "https://api-stage-samsung.pulsellc.com/api/v1/gaming/Updates";

        /// <summary>
        /// Sends a POST request to the gaming API.
        /// </summary>
        /// <param name="eid">The authorization token.</param>
        /// <param name="gameName">Name of the game.</param>
        /// <param name="score">Score of the player.</param>
        /// <param name="earnedPoints">Points earned by the player.</param>
        /// <param name="quizResult">The quiz result.</param>
        public void SendGameData(string eid, string gameName, string score, int earnedPoints, string quizResult)
        {
            StartCoroutine(PostGameData(eid, gameName, score, earnedPoints, quizResult));
        }

        private IEnumerator PostGameData(string eid, string gameName, string score, int earnedPoints, string quizResult)
        {
            // Create the payload (without the 'eid' token here)
            var payload = new
            {
                GameName = gameName,
                Score = score,
                EarnedPoints = earnedPoints,
                QuizResult = quizResult
            };

            // Serialize the payload to JSON
            string jsonPayload = JsonUtility.ToJson(payload);

            // Create the UnityWebRequest
            using (UnityWebRequest request = new UnityWebRequest(BaseURL, "POST"))
            {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Authorization", $"Basic {eid}"); // Set the Authorization header with the provided eid token

                Debug.Log("Sending request to API...");

                // Send the request and wait for a response
                yield return request.SendWebRequest();

                // Handle the response
                if (request.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log($"Response: {request.responseCode} - Success");
                }
                else
                {
                    Debug.LogError($"Response: {request.responseCode} - Error: {request.error}");
                }
            }
        }
    }
}
