using UnityEngine;
using UnityEngine.InputSystem;

namespace FictionalOctoDoodle.Core
{
    public class ClimbingState : PlayerState
    {
        public override PlayerStateID ID => PlayerStateID.Climbing;

        private InputAction movement;
        private PlayerMovement player;

        public override void EnterState(PlayerMovement player)
        {
            Debug.Log("Climbing");
            this.player = player;
            movement = player.Input.Player.Move;
            player.ToggleGravity(false);
        }

        public override void Update()
        {
            var value = movement.ReadValue<Vector2>();

            if (player.canClimb)
            {
                player.Move(value);
                return;
            }

            if (!player.IsGrounded())
            {
                player.SetNewState(new AirborneState());
                return;
            }
            else
            {
                player.SetNewState(
                    value == Vector2.zero ?
                    new IdleState() :
                    new RunningState()
                    );
            }
        }

        public override void ExitState()
        {
            player.ToggleGravity(true);
        }
    }
}

