using UnityEngine;
using UnityEngine.SceneManagement;

namespace FictionalOctoDoodle.Core
{
    public class GameMenus : MonoBehaviour
    {
        [SerializeField] string gameSceneName;
        [SerializeField] string menuSceneName;
        

        public void GoToMainMenu()
        {
            SceneManager.LoadScene(menuSceneName, LoadSceneMode.Single); // will want to change this to additive later
        }

        public void StartGame()
        {
            SceneManager.LoadScene(gameSceneName, LoadSceneMode.Single);
        }
        
        public void QuitProgram()
        {
            Application.Quit();
        }
    }
}

