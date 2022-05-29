using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class AttackingState : PlayerState
    {
        public override PlayerStateID ID => PlayerStateID.Attacking;
        private PlayerMovement player;

        public override void EnterState(PlayerMovement player)
        {
            Debug.Log("Attacking");
            this.player = player;
            player.Input.Player.Jump.Disable();
        }

        public override void ExitState()
        {
            player.Input.Player.Jump.Enable();
        }
    }
}

