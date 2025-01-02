using TMPro;
using UnityEngine;

namespace GalaxyGridiron
{
    /// <summary>
    /// This answerBox handles the appearance of the info when we have selected 
    /// one of the answers (either correct or incorrect)
    /// </summary>
    public class AnswerBox : MonoBehaviour
    {
        /*
        Result (Correct/Incorrect)
        Heading (PlayerResults)
        Description (You made it to total-randomYards time)
        YardsGained
        YardsToFirstDown
        */
        [SerializeField] TMP_Text _resultText;
        [SerializeField] TMP_Text _headingText;
        [SerializeField] TMP_Text _descriptionText;
        [SerializeField] TMP_Text _yardsGained;
        [SerializeField] TMP_Text _yardsToFirstDown;

        public void OpenBox()
        {
            Debug.Log($"Box called, open");
            GetComponent<Animator>().CrossFade("Appear", .1f);
        }

        [ContextMenu("CloseBox")]
        public void CloseBox()
        {
            Debug.Log($"Box called, false");
            GetComponent<Animator>().CrossFade("Default", .1f);
        }
        public void SetInfo(UserResult userResult)
        {
            _resultText.text = userResult.result;
            _headingText.text = userResult.heading;
            _descriptionText.text = userResult.description;
            _yardsGained.text = userResult.yardsGained;
            _yardsToFirstDown.text = userResult.yardsToFirstDown;
        }
    }


    /// <summary>
    /// This result class contains the data which is used afterwards the user
    /// has opted for any of the options
    /// </summary>
    public class UserResult
    {
        public string result { get; set; }
        public string heading { get; set; }
        public string description { get; set; }
        public string yardsGained { get; set; }
        public string yardsToFirstDown { get; set; }
    }
}
