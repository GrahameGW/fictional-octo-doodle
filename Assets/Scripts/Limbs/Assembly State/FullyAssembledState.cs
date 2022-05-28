using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class FullyAssembledState : LimbAssemblyState
    {
        public override bool AddLimb(LimbData limb)
        {
            return false;
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

            context.RemoveLimb(limb);
            RefreshAssembly(ctrl, stats, state);
            return true;
        }
    }
}

