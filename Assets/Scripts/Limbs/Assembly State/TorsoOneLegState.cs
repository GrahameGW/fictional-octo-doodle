using System.Linq;
using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class TorsoOneLegState : LimbAssemblyState
    {
        public override bool AddLimb(LimbData limb)
        {
            if (!limb.Slots.Any(s => s != LimbSlot.Torso || s == LimbSlot.FrontLeg)) return false;
            var slot = limb.Slots.First(s => s == LimbSlot.BackLeg || s == LimbSlot.FrontArm);

            LimbAssemblyState state;
            RuntimeAnimatorController ctrl;

            if (slot == LimbSlot.BackLeg)
            {
                state = new TorsoTwoLegState();
                ctrl = context.controllers.torsoTwoLeg;
            }
            else
            {
                state = new OneArmOneLegState();
                ctrl = context.controllers.oneLegOneArm;
            }

            context.AssembleLimb(limb, slot);
            context.ChangeState(state);
            context.SetAnimationController(ctrl);
            return true;
        }

        public override bool RemoveLimb(LimbSlot limb)
        {
            if (limb != LimbSlot.FrontLeg) return false;

            context.RemoveLimb(limb);
            context.ChangeState(new SkullAndTorsoState());
            context.SetAnimationController(context.controllers.skullTorso);
            return true;
        }
    }
}

