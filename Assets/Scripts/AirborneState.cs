using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class AirborneState : PlayerState
    {
        public override PlayerStateID ID => PlayerStateID.Airborne;

        private Player player;
        private float lastMovement;

        public override void EnterState(Player player)
        {
            Debug.Log("Airborne");
            this.player = player;
            lastMovement = player.Input.Player.Move.ReadValue<Vector2>().x;
            Debug.Log($"Last Movement: {lastMovement}");
        }

        public override void Update()
        {
            if (player.IsGrounded())
            {
                player.SetNewState(lastMovement == 0 ? new IdleState() : new MovingState());
                return;
            }

            player.Move(lastMovement);
        }
    }
}

