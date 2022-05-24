using UnityEngine;
using UnityEngine.InputSystem;

namespace FictionalOctoDoodle.Core
{
    public class SwimmingState : PlayerState
    {
        public override PlayerStateID ID => PlayerStateID.Idle;

        private InputAction movement;
        private PlayerMovement player;

        public override void EnterState(PlayerMovement player)
        {
            Debug.Log("Swimming");
            this.player = player;
            movement = player.Input.Player.Move;
            player.Input.Player.Jump.Disable();
            player.ToggleGravity(0f);
        }

        public override void Update()
        {
            var value = movement.ReadValue<Vector2>();
            value += new Vector2(0f, -0.5f); // "gravity"
            player.Move(value);

            if (!player.InWater)
            {
                if (!player.IsGrounded())
                {
                    player.SetNewState(new AirborneState());
                }
                else
                {
                    player.SetNewState(value == Vector2.zero ?
                        new IdleState() :
                        new RunningState()
                        );
                }
            }
        }

        public override void ExitState()
        {
            player.ToggleGravity(1);
            player.Input.Player.Jump.Enable();
        }
    }
}

