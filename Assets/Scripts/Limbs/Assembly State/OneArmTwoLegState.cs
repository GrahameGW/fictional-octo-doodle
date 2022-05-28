using System.Linq;
using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class OneArmTwoLegState : LimbAssemblyState
    {
        public override bool AddLimb(LimbData limb)
        {
            if (!limb.Slots.Contains(LimbSlot.BackArm)) return false;

            context.AssembleLimb(limb, LimbSlot.BackArm);
            context.ChangeState(new FullyAssembledState());
            context.SetAnimationController(context.controllers.fullBody);
            return true;
        }

        public override bool RemoveLimb(LimbSlot limb)
        {
            if (limb == LimbSlot.Torso || limb == LimbSlot.BackArm) return false;

            LimbAssemblyState state;
            RuntimeAnimatorController ctrl;

            if (limb == LimbSlot.FrontArm)
            {
                state = new TorsoTwoLegState();
                ctrl = context.controllers.torsoTwoLeg;
            }
            else
            {
                state = new OneArmOneLegState();
                ctrl = context.controllers.oneLegOneArm;
            }

            context.RemoveLimb(limb);
            context.ChangeState(state);
            context.SetAnimationController(ctrl);
            return true;
        }
    }
}

