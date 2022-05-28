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
            PlayerMoveStats stats;

            if (slot == LimbSlot.BackLeg)
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

            context.AssembleLimb(limb, slot);
            RefreshAssembly(ctrl, stats, state);
            return true;
        }

        public override bool RemoveLimb(LimbSlot limb)
        {
            if (limb != LimbSlot.FrontLeg) return false;

            context.RemoveLimb(limb);
            RefreshAssembly(context.controllers.skullTorso, context.moveStats.skullTorso, new SkullAndTorsoState());
            return true;
        }
    }
}

