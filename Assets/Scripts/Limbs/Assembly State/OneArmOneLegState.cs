using System.Linq;
using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class OneArmOneLegState : LimbAssemblyState
    {
        public override bool AddLimb(LimbData limb)
        {
            if (!limb.Slots.Any(s => s == LimbSlot.BackArm || s == LimbSlot.BackLeg)) return false;
            var slot = limb.Slots.First(s => s == LimbSlot.BackArm || s == LimbSlot.BackLeg);

            LimbAssemblyState state;
            RuntimeAnimatorController ctrl;

            if (slot == LimbSlot.BackArm) 
            {
                state = new TwoArmOneLegState();
                ctrl = context.controllers.oneLegTwoArm;
            }
            else
            {
                state = new OneArmTwoLegState();
                ctrl = context.controllers.twoLegOneArm;
            }

            context.AssembleLimb(limb, slot);
            context.ChangeState(state);
            context.SetAnimationController(ctrl);
            return true;
        }

        public override bool RemoveLimb(LimbSlot limb)
        {
            if (limb == LimbSlot.Torso) return false;

            LimbAssemblyState state;
            RuntimeAnimatorController ctrl;

            if (limb == LimbSlot.BackArm || limb == LimbSlot.FrontArm)
            {
                state = new TorsoOneLegState();
                ctrl = context.controllers.torsoOneLeg;
            }
            else
            {
                state = new TorsoOneArmState();
                ctrl = context.controllers.torsoOneArm;
            }
            context.RemoveLimb(limb);
            context.ChangeState(state);
            context.SetAnimationController(ctrl);
            return true;
        }
    }
}

