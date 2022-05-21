using UnityEngine;
using UnityEngine.InputSystem;

namespace FictionalOctoDoodle.Core
{
    public class IdleState : PlayerState
    {
        public override PlayerStateID ID => PlayerStateID.Idle;

        private InputAction movement;
        private PlayerMovement player;


        public override void EnterState(PlayerMovement player)
        {
            Debug.Log("Standing");
            this.player = player;
            movement = player.Input.Player.Move;
        }
        public override void Update()
        {
            if (!player.IsGrounded())
            {
                player.SetNewState(new AirborneState());
                return;
            }

            var value = movement.ReadValue<Vector2>();

            if (value != Vector2.zero)
            {
                player.Move(value.x);
                player.SetNewState(new MovingState());
            }


        }
    }
}

