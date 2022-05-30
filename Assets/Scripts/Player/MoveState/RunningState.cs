using UnityEngine;
using UnityEngine.InputSystem;

namespace FictionalOctoDoodle.Core
{
    public class RunningState : PlayerState
    {
        public override PlayerStateID ID => PlayerStateID.Running;

        private PlayerMovement context;
        private Player player;
        private InputAction movement;


        public override void EnterState(PlayerMovement playerMove)
        {
            Debug.Log("Running");
            context = playerMove;

            movement = playerMove.Input.Player.Move;
            player = context.GetComponent<Player>();
            player.FootstepsPlayback = true;
        }

        public override void Update()
        {
            var value = movement.ReadValue<Vector2>();
            context.Move(new Vector2(value.x, 0f));


            if (value.y != 0 && context.CanClimb)
            {
                context.SetNewState(new ClimbingState());
                return;
            }

            if (value.x == 0)
            {
                context.SetNewState(new IdleState());
                return;
            }

            if (!context.IsGrounded())
            {
                context.SetNewState(new AirborneState());
                return;
            }

            if (context.InWater)
            {
                context.SetNewState(new SwimmingState());
                return;
            }
        }

        public override void ExitState()
        {
            player.FootstepsPlayback = false;
        }
    }
}

