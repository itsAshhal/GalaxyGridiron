using UnityEngine;

namespace GalaxyGridiron
{
    using System;
    using System.IO;
    using UnityEngine;

    public static class FileHandle
    {
        /// <summary>
        /// Saves data to a file in the StreamingAssets folder.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="data">The string data to save.</param>
        public static void SaveToFile(string fileName, string data)
        {
            string path = Path.Combine(Application.streamingAssetsPath, fileName);

            try
            {
                File.WriteAllText(path, data);
                Debug.Log($"File saved successfully at: {path}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save file: {e.Message}");
            }
        }

        /// <summary>
        /// Reads data from a file in the StreamingAssets folder.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <returns>The string data read from the file, or null if an error occurred.</returns>
        public static string LoadFromFile(string fileName)
        {
            string path = Path.Combine(Application.streamingAssetsPath, fileName);

            try
            {
                if (File.Exists(path))
                {
                    return File.ReadAllText(path);
                }
                else
                {
                    Debug.LogWarning($"File not found at: {path}");
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load file: {e.Message}");
                return null;
            }
        }
    }

    public static class GameData
    {
        /// <summary>
        /// Saves a serializable object as a JSON string to a file.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="data">The object to save.</param>
        public static void Save<T>(string fileName, T data) where T : class
        {
            try
            {
                string jsonData = JsonUtility.ToJson(data, true);
                FileHandle.SaveToFile(fileName, jsonData);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save data: {e.Message}");
            }
        }

        /// <summary>
        /// Loads a JSON string from a file and deserializes it into an object.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="fileName">The name of the file.</param>
        /// <returns>The deserialized object, or null if an error occurred.</returns>
        public static T Load<T>(string fileName) where T : class
        {
            try
            {
                string jsonData = FileHandle.LoadFromFile(fileName);
                if (!string.IsNullOrEmpty(jsonData))
                {
                    return JsonUtility.FromJson<T>(jsonData);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load data: {e.Message}");
            }

            return null;
        }
    }

}