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
            PlayerMoveStats stats;

            if (slot == LimbSlot.BackArm) 
            {
                state = new TwoArmOneLegState();
                ctrl = context.controllers.oneLegTwoArm;
                stats = context.moveStats.oneLegTwoArm;
            }
            else
            {
                state = new OneArmTwoLegState();
                ctrl = context.controllers.twoLegOneArm;
                stats = context.moveStats.twoLegOneArm;
            }

            context.AssembleLimb(limb, slot);
            RefreshAssembly(ctrl, stats, state);
            return true;
        }

        public override bool RemoveLimb(LimbSlot limb)
        {
            if (limb == LimbSlot.Torso) return false;

            LimbAssemblyState state;
            RuntimeAnimatorController ctrl;
            PlayerMoveStats stats;

            if (limb == LimbSlot.BackArm || limb == LimbSlot.FrontArm)
            {
                state = new TorsoOneLegState();
                ctrl = context.controllers.torsoOneLeg;
                stats = context.moveStats.torsoOneLeg;
            }
            else
            {
                state = new TorsoOneArmState();
                ctrl = context.controllers.torsoOneArm;
                stats = context.moveStats.torsoOneArm;
            }
            context.RemoveLimb(limb);
            RefreshAssembly(ctrl, stats, state);
            return true;
        }
    }
}

