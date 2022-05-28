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

            if (limb == LimbSlot.FrontArm || limb == LimbSlot.BackArm)
            {
                state = new OneArmTwoLegState();
                ctrl = context.controllers.twoLegOneArm;
            }
            else
            {
                state = new TwoArmOneLegState();
                ctrl = context.controllers.oneLegTwoArm;
            }

            context.RemoveLimb(limb);
            context.ChangeState(state);
            context.SetAnimationController(ctrl);
            return true;
        }
    }
}

