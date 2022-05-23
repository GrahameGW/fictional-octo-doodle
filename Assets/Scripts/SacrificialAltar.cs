using UnityEngine;
using UnityEngine.InputSystem;

namespace FictionalOctoDoodle.Core
{
    public class SacrificialAltar : MonoBehaviour
    {
        private PlayerInputMap input;
        private Player player;

        private void Awake()
        {
            input = new PlayerInputMap();
        }

        private void Sacrifice(InputAction.CallbackContext ctx)
        {
            Debug.Log("Sacrificed yourself!");
            player.Damage(int.MaxValue);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var player = collision.gameObject.GetComponentInParent<Player>();

            if (player != null)
            {
                input.Player.Submit.performed += Sacrifice;
                input.Player.Submit.Enable();
                this.player = player;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            var player = collision.gameObject.GetComponentInParent<Player>();

            if (player != null)
            {
                input.Player.Submit.performed -= Sacrifice;
                input.Player.Submit.Disable();
                this.player = null;
            }
        }
    }
}

