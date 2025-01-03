using UnityEngine;
using UnityEngine.SceneManagement;

namespace GalaxyGridiron
{
    public class SceneController : MonoBehaviour
    {
        public void LoadScene(int sceneIndex) { SceneManager.LoadScene(sceneIndex); }
        public void LoadScene(string sceneName) { SceneManager.LoadScene(sceneName); }
    }
}
