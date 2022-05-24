using UnityEngine;
using UnityEngine.InputSystem;

namespace FictionalOctoDoodle.Core
{
    public class RunningState : PlayerState
    {
        public override PlayerStateID ID => PlayerStateID.Running;

        private PlayerMovement player;
        private InputAction movement;


        public override void EnterState(PlayerMovement player)
        {
            Debug.Log("Running");
            this.player = player;

            movement = player.Input.Player.Move;
        }

        public override void Update()
        {
            var value = movement.ReadValue<Vector2>();
            player.Move(new Vector2(value.x, 0f));


            if (value.y != 0 && player.CanClimb)
            {
                player.SetNewState(new ClimbingState());
                return;
            }

            if (value.x == 0)
            {
                player.SetNewState(new IdleState());
                return;
            }

            if (!player.IsGrounded())
            {
                player.SetNewState(new AirborneState());
                return;
            }

            if (player.InWater)
            {
                player.SetNewState(new SwimmingState());
            }
        }
    }
}

