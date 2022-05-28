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
            RefreshAssembly(context.controllers.fullBody, context.moveStats.fullBody, new FullyAssembledState());
            return true;
        }

        public override bool RemoveLimb(LimbSlot limb)
        {
            if (limb == LimbSlot.Torso || limb == LimbSlot.BackLeg) return false;

            LimbAssemblyState state;
            RuntimeAnimatorController ctrl;
            PlayerMoveStats stats;

            if (limb == LimbSlot.FrontLeg)
            {
                state = new TorsoTwoArmState();
                ctrl = context.controllers.torsoTwoArm;
                stats = context.moveStats.torsoTwoArm;
            }
            else
            {
                state = new OneArmOneLegState();
                ctrl = context.controllers.oneLegOneArm;
                stats = context.moveStats.torsoTwoArm;
            }

            context.RemoveLimb(limb);
            RefreshAssembly(ctrl, stats, state);
            return true;
        }
    }
}

