/*using System;
using Unity.VisualScripting;
using UnityEngine;

namespace GalaxyGridiron
{
    public class DataController : MonoBehaviour
    {
        public string fileName = string.Empty;
        public static DataController Instance;
        [SerializeField] QuestionBase question;

        private void Awake()
        {
            Instance = this;
        }

        public void SaveData()
        {

        }


        [ContextMenu("LoadData from JSON")]
        public void LoadData()
        {
            var data = GameData.Load<UserData>("UserData.json");
            Debug.Log($"Total length we have {data.questions.Length}");

            foreach (var question in data.questions)
            {
                Debug.Log($"Question is {question.question}");
                foreach (var option in question.options)
                {
                    Debug.Log($"Options are {option}");
                }
                Debug.Log($"Correct answer is {question.options[question.correctOptionIndex]}");
            }
        }
    }
}



*/