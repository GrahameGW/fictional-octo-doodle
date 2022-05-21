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
            lastMovement = movement.ReadValue<Vector2>();
            player.Input.Player.Jump.Disable();
        }

        public override void Update()
        {
            if (player.IsGrounded())
            {
                Debug.Log("Landed");
                player.SetNewState(lastMovement == Vector2.zero ? new IdleState() : new RunningState());
                return;
            }

            if (player.canClimb && movement.ReadValue<Vector2>().y != 0)
            {
                player.SetNewState(new ClimbingState());
                return;
            }

            player.Move(lastMovement);
        }

        public override void ExitState()
        {
            player.Input.Player.Move.Enable();
            player.Input.Player.Jump.Enable();
        }

    }
}

