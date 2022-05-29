using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class FullyAssembledState : LimbAssemblyState
    {
        public override bool AddLimb(LimbData limb)
        {
            var slot = limb.Slots[Random.Range(0, limb.Slots.Length)];

            context.RemoveLimb(slot, true);
            context.AssembleLimb(limb, slot);
            RefreshAssembly(context.controllers.fullBody, context.moveStats.fullBody, this);
            return true;
        }

        public override bool RemoveLimb(LimbSlot limb)
        {
            if (limb == LimbSlot.Torso) return false;

            LimbAssemblyState state;
            RuntimeAnimatorController ctrl;
            PlayerMoveStats stats;

            if (limb == LimbSlot.FrontArm || limb == LimbSlot.BackArm)
            {
                state = new OneArmTwoLegState();
                ctrl = context.controllers.twoLegOneArm;
                stats = context.moveStats.twoLegOneArm;
            }
            else
            {
                state = new TwoArmOneLegState();
                ctrl = context.controllers.oneLegTwoArm;
                stats = context.moveStats.oneLegTwoArm;
            }

            context.RemoveLimb(limb, true);
            RefreshAssembly(ctrl, stats, state);
            return true;
        }
    }
}

