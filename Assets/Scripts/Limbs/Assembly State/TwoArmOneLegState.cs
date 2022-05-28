using System.Linq;
using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class TwoArmOneLegState : LimbAssemblyState
    {
        public override bool AddLimb(LimbData limb)
        {
            if (!limb.Slots.Contains(LimbSlot.BackLeg)) return false;

            context.AssembleLimb(limb, LimbSlot.BackLeg);
            context.ChangeState(new FullyAssembledState());
            context.SetAnimationController(context.controllers.fullBody);
            return true;
        }

        public override bool RemoveLimb(LimbSlot limb)
        {
            if (limb == LimbSlot.Torso || limb == LimbSlot.BackLeg) return false;


            LimbAssemblyState state;
            RuntimeAnimatorController ctrl;

            if (limb == LimbSlot.FrontLeg)
            {
                state = new TorsoTwoArmState();
                ctrl = context.controllers.torsoTwoArm;
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

