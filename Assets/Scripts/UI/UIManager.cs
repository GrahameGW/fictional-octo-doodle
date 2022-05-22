using UnityEngine;
using UnityEngine.InputSystem;

namespace FictionalOctoDoodle.Core
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] GameObject pauseMenu;
        
        private PlayerInputMap input;


        private void OnEnable()
        {
            input = new PlayerInputMap();
            input.UI.Cancel.performed += PauseGame;
            input.UI.Cancel.Enable();
        }

        private void OnDisable()
        {
            input.UI.Cancel.performed -= PauseGame;
            input.UI.Cancel.Disable();
        }


        private void PauseGame(InputAction.CallbackContext ctx)
        {
            pauseMenu.SetActive(true);
        }
    }
}

