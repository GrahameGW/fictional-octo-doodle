using System.Linq;
using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class TorsoTwoLegState : LimbAssemblyState
    {
        public override bool AddLimb(LimbData limb)
        {
            if (!limb.Slots.Contains(LimbSlot.FrontArm))
            {
                if (limb.Slots.Length == 1 && limb.Slots[0] == LimbSlot.BackArm) return false;

                var slot = limb.Slots[Random.Range(0, limb.Slots.Length)];

                context.RemoveLimb(slot, true);
                context.AssembleLimb(limb, slot);
                RefreshAssembly(context.controllers.torsoTwoLeg, context.moveStats.torsoTwoLeg, this);
                return true;
            }

            context.AssembleLimb(limb, LimbSlot.FrontArm);
            RefreshAssembly(context.controllers.twoLegOneArm, context.moveStats.twoLegOneArm, new OneArmTwoLegState());

            return true;
        }

        public override bool RemoveLimb(LimbSlot limb)
        {
            if (limb != LimbSlot.FrontLeg || limb != LimbSlot.BackLeg)
            {
                return false;
            }

            context.RemoveLimb(limb, true);
            RefreshAssembly(context.controllers.torsoOneLeg, context.moveStats.torsoOneLeg, new TorsoOneLegState());
            return true;
        }
    }
}

