using System.Linq;
using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class SkullAndTorsoState : LimbAssemblyState
    {
        public override bool AddLimb(LimbData limb)
        {
            if (!limb.Slots.Any(s => s != LimbSlot.Torso)) return false;
            var slot = limb.Slots.First(s => s != LimbSlot.Torso);

            LimbAssemblyState state;
            RuntimeAnimatorController ctrl;

            if (slot == LimbSlot.BackArm || slot == LimbSlot.FrontArm)
            {
                state = new TorsoOneArmState();
                ctrl = context.controllers.torsoOneArm;
            }
            else
            {
                state = new TorsoOneLegState();
                ctrl = context.controllers.torsoOneLeg;
            }

            context.AssembleLimb(limb, slot);
            context.ChangeState(state);
            context.SetAnimationController(ctrl);
            return true;
        }

        public override bool RemoveLimb(LimbSlot limb)
        {
            if (limb != LimbSlot.Torso) return false;

            context.RemoveLimb(limb);
            context.ChangeState(new SkullOnlyState());
            context.SetAnimationController(context.controllers.skullOnly);
            return true;
        }
    }
}

