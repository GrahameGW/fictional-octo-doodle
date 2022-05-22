using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FictionalOctoDoodle.Core
{
    public class PauseMenu : MonoBehaviour
    {
        private void OnEnable()
        {
            Time.timeScale = 0;

        }

        private void OnDisable()
        {
            Time.timeScale = 1;
            EventSystem.current.SetSelectedGameObject(null); // because unity is buggy and doesn't deselect the button...
        }

        public void QuitProgram()
        {
            Application.Quit();
        }
    }
}

