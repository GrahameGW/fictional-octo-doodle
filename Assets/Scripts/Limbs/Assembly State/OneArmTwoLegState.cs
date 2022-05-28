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
            RefreshAssembly(context.controllers.fullBody, context.moveStats.fullBody, new FullyAssembledState());
            return true;
        }

        public override bool RemoveLimb(LimbSlot limb)
        {
            if (limb == LimbSlot.Torso || limb == LimbSlot.BackArm) return false;

            LimbAssemblyState state;
            RuntimeAnimatorController ctrl;
            PlayerMoveStats stats;

            if (limb == LimbSlot.FrontArm)
            {
                state = new TorsoTwoLegState();
                ctrl = context.controllers.torsoTwoLeg;
                stats = context.moveStats.torsoTwoLeg;
            }
            else
            {
                state = new OneArmOneLegState();
                ctrl = context.controllers.oneLegOneArm;
                stats = context.moveStats.oneLegOneArm;
            }

            context.RemoveLimb(limb);
            RefreshAssembly(ctrl, stats, state);
            return true;
        }
    }
}

