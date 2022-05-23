using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace FictionalOctoDoodle.Core
{
    public class GameMenus : MonoBehaviour
    {
        [SerializeField] string gameSceneName;
        [SerializeField] string menuSceneName;
        
        private void OnDisable()
        {
            EventSystem.current?.SetSelectedGameObject(null); // because unity is buggy and doesn't deselect the button...
        }

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

