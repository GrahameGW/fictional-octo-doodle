using UnityEngine;
using UnityEngine.InputSystem;

namespace FictionalOctoDoodle.Core
{
    public class MovingState : PlayerState
    {
        public override PlayerStateID ID => PlayerStateID.Moving;

        private PlayerMovement player;
        private InputAction movement;

        public override void EnterState(PlayerMovement player)
        {
            Debug.Log("Moving");
            this.player = player;

            movement = player.Input.Player.Move;
        }

        public override void Update()
        {
            var value = movement.ReadValue<Vector2>();
            player.Move(value.x);

            if (value == Vector2.zero)
            {
                player.SetNewState(new IdleState());
            }
        }
    }
}

