using UnityEngine;
using UnityEngine.InputSystem;

namespace FictionalOctoDoodle.Core
{
    public class MovingState : PlayerState
    {
        public override PlayerStateID ID => PlayerStateID.Moving;

        private Player player;
        private InputAction movement;

        public override void EnterState(Player player)
        {
            Debug.Log("Moving");
            this.player = player;
            movement = player.Input.Player.Move;
            movement.Enable();
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
        public override void ExitState()
        {
            movement.Disable();
        }
    }
}

