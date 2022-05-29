using UnityEngine;
using UnityEngine.InputSystem;

namespace FictionalOctoDoodle.Core
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] GameObject pauseMenu;
        [SerializeField] GameObject hud;

        private PlayerInputMap input;
        private bool isPaused;

        private void OnEnable()
        {
            input = new PlayerInputMap();
            input.UI.LoadMenu.performed += TogglePause;
            input.UI.LoadMenu.Enable();
        }

        private void OnDisable()
        {
            input.UI.LoadMenu.performed -= TogglePause;
            input.UI.LoadMenu.Disable();
        }


        private void TogglePause(InputAction.CallbackContext ctx)
        {
            isPaused = !isPaused;
            pauseMenu.SetActive(isPaused);
            hud.SetActive(!isPaused);
            Time.timeScale = isPaused ? 0 : 1;
        }

        public void TogglePause()
        {
            TogglePause(new InputAction.CallbackContext());
        }
    }
}

