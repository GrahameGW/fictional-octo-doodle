using UnityEngine;
using UnityEngine.InputSystem;

namespace FictionalOctoDoodle.Core
{
    public class AirborneState : PlayerState
    {
        public override PlayerStateID ID => PlayerStateID.Airborne;

        private PlayerMovement player;
        private InputAction movement;
        private Vector2 lastMovement;

        public override void EnterState(PlayerMovement player)
        {
            Debug.Log("Airborne");
            this.player = player;
            movement = player.Input.Player.Move;
            //lastMovement = movement.ReadValue<Vector2>();
            player.Input.Player.Jump.Disable();
        }

        public override void Update()
        {
            var value = movement.ReadValue<Vector2>();
            //  player.Move(lastMovement);
            player.Move(new Vector2(value.x, 0f));

            if (player.IsGrounded())
            {
                Debug.Log("Landed");
                player.SetNewState(value == Vector2.zero ? new IdleState() : new RunningState());
                return;
            }

            if (player.CanClimb && value.y != 0)
            {
                player.SetNewState(new ClimbingState());
                return;
            }

            if (player.InWater)
            {
                player.SetNewState(new SwimmingState());
                return;
            }
        }

        public override void ExitState()
        {
            player.Input.Player.Move.Enable();
            player.Input.Player.Jump.Enable();
        }

    }
}

