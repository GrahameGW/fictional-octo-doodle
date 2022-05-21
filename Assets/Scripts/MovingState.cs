using UnityEngine;
using UnityEngine.InputSystem;

namespace FictionalOctoDoodle.Core
{
    public class MovingState : PlayerState
    {
        public override PlayerStateID ID => PlayerStateID.Moving;

        private InputAction movement;

        public override void EnterState(PlayerInputMap inputMap)
        {
            Debug.Log("Moving");
            movement = inputMap.Player.Move;
            movement.Enable();
        }
        public override void Update(Player player)
        {
            var value = movement.ReadValue<Vector2>();
            player.Move(value);

            if (value == Vector2.zero)
            {
                player.SetNewState(new IdleState());
            }
        }
        public override void ExitState()
        {
            movement.Disable();
        }
    }
}

