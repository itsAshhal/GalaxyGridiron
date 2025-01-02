using System;
using UnityEngine;

/// <summary>
/// Defined everything you need to save into and load from the JSON.
/// You need to define the data model classes that you want to save and load (talking about QuestionBase here).
/// </summary>
[Serializable]
public class UserData
{
    public QuestionBase[] questions;
}


/// <summary>
/// This is the main baseQuestion dataModel which has a question and then its options
/// and then the correct answer
/// </summary>
[Serializable]
public class QuestionBase
{
    public string question;
    public string[] options = new string[4];
    public int correctOptionIndex;

}



// extra

public class QuestionData
{
    public static UserData GetSampleQuestions()
    {
        UserData userData = new UserData();
        userData.questions = new QuestionBase[15]; // Array to hold 15 questions

        // Question 1
        userData.questions[0] = new QuestionBase
        {
            question = "What is the capital of France?",
            options = new string[] { "Berlin", "Madrid", "Paris", "Rome" },
            correctOptionIndex = 2 // Paris is the correct answer
        };

        // Question 2
        userData.questions[1] = new QuestionBase
        {
            question = "Which planet is known as the Red Planet?",
            options = new string[] { "Earth", "Mars", "Jupiter", "Saturn" },
            correctOptionIndex = 1 // Mars is the correct answer
        };

        // Question 3
        userData.questions[2] = new QuestionBase
        {
            question = "What is the largest ocean on Earth?",
            options = new string[] { "Atlantic Ocean", "Indian Ocean", "Pacific Ocean", "Arctic Ocean" },
            correctOptionIndex = 2 // Pacific Ocean is the correct answer
        };

        // Question 4
        userData.questions[3] = new QuestionBase
        {
            question = "Which is the smallest country in the world?",
            options = new string[] { "Monaco", "Vatican City", "San Marino", "Liechtenstein" },
            correctOptionIndex = 1 // Vatican City is the correct answer
        };

        // Question 5
        userData.questions[4] = new QuestionBase
        {
            question = "What is the chemical symbol for water?",
            options = new string[] { "O2", "CO2", "H2O", "HO2" },
            correctOptionIndex = 2 // H2O is the correct answer
        };

        // Question 6
        userData.questions[5] = new QuestionBase
        {
            question = "Who wrote 'Romeo and Juliet'?",
            options = new string[] { "William Shakespeare", "Charles Dickens", "Jane Austen", "Mark Twain" },
            correctOptionIndex = 0 // William Shakespeare is the correct answer
        };

        // Question 7
        userData.questions[6] = new QuestionBase
        {
            question = "What is the largest land animal?",
            options = new string[] { "Elephant", "Giraffe", "Rhinoceros", "Hippo" },
            correctOptionIndex = 0 // Elephant is the correct answer
        };

        // Question 8
        userData.questions[7] = new QuestionBase
        {
            question = "What is the square root of 64?",
            options = new string[] { "6", "7", "8", "9" },
            correctOptionIndex = 2 // 8 is the correct answer
        };

        // Question 9
        userData.questions[8] = new QuestionBase
        {
            question = "What is the tallest mountain in the world?",
            options = new string[] { "K2", "Mount Kilimanjaro", "Mount Everest", "Mount Fuji" },
            correctOptionIndex = 2 // Mount Everest is the correct answer
        };

        // Question 10
        userData.questions[9] = new QuestionBase
        {
            question = "Which ocean is on the west coast of the United States?",
            options = new string[] { "Atlantic Ocean", "Pacific Ocean", "Indian Ocean", "Southern Ocean" },
            correctOptionIndex = 1 // Pacific Ocean is the correct answer
        };

        // Question 11
        userData.questions[10] = new QuestionBase
        {
            question = "Who discovered America?",
            options = new string[] { "Christopher Columbus", "Marco Polo", "Vasco da Gama", "Ferdinand Magellan" },
            correctOptionIndex = 0 // Christopher Columbus is the correct answer
        };

        // Question 12
        userData.questions[11] = new QuestionBase
        {
            question = "What is the chemical symbol for gold?",
            options = new string[] { "Ag", "Au", "Fe", "Pb" },
            correctOptionIndex = 1 // Au is the correct answer
        };

        // Question 13
        userData.questions[12] = new QuestionBase
        {
            question = "What is the main ingredient in guacamole?",
            options = new string[] { "Tomato", "Avocado", "Onion", "Garlic" },
            correctOptionIndex = 1 // Avocado is the correct answer
        };

        // Question 14
        userData.questions[13] = new QuestionBase
        {
            question = "What is the fastest land animal?",
            options = new string[] { "Cheetah", "Lion", "Horse", "Elephant" },
            correctOptionIndex = 0 // Cheetah is the correct answer
        };

        // Question 15
        userData.questions[14] = new QuestionBase
        {
            question = "Which country is known as the Land of the Rising Sun?",
            options = new string[] { "China", "Japan", "South Korea", "India" },
            correctOptionIndex = 1 // Japan is the correct answer
        };

        return userData;
    }
}
