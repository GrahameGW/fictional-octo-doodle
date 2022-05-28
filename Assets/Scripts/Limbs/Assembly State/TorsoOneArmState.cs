using System.Linq;
using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class TorsoOneArmState : LimbAssemblyState
    {
        public override bool AddLimb(LimbData limb)
        {
            if (!limb.Slots.Any(s => s != LimbSlot.Torso || s != LimbSlot.FrontArm)) return false;
            var slot = limb.Slots.First(s => s == LimbSlot.FrontLeg || s == LimbSlot.BackArm);

            LimbAssemblyState state;
            RuntimeAnimatorController ctrl;

            if (slot == LimbSlot.BackArm)
            {
                state = new TorsoTwoArmState();
                ctrl = context.controllers.torsoTwoArm;
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
            if (limb != LimbSlot.FrontArm) return false;

            context.RemoveLimb(limb);
            context.ChangeState(new SkullAndTorsoState());
            context.SetAnimationController(context.controllers.skullTorso);
            return true;
        }
    }
}

