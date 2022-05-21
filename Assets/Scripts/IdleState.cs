using UnityEngine;
using UnityEngine.InputSystem;

namespace FictionalOctoDoodle.Core
{
    public class IdleState : PlayerState
    {
        public override PlayerStateID ID => PlayerStateID.Idle;

        public InputAction movement;

        public override void EnterState(PlayerInputMap inputMap)
        {
            Debug.Log("Standing");
            movement = inputMap.Player.Move;
            movement.Enable();
        }
        public override void Update(Player player)
        {
            var value = movement.ReadValue<Vector2>();

            if (value != Vector2.zero)
            {
                player.Move(value);
                player.SetNewState(new MovingState());
            }
        }
        public override void ExitState()
        {
            movement.Disable();
        }
    }
}

