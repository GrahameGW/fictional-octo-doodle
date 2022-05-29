using UnityEngine;
using System.Linq;

namespace FictionalOctoDoodle.Core
{
    public class TorsoTwoArmState : LimbAssemblyState
    {
        public override bool AddLimb(LimbData limb)
        {
            if (!limb.Slots.Contains(LimbSlot.FrontLeg))
            {
                if (limb.Slots.Length == 1 && limb.Slots[0] == LimbSlot.BackLeg) return false;

                var slot = limb.Slots[Random.Range(0, limb.Slots.Length)];

                context.RemoveLimb(slot, true);
                context.AssembleLimb(limb, slot);
                RefreshAssembly(context.controllers.torsoTwoArm, context.moveStats.torsoTwoArm, this);
                return true;
            }

            context.AssembleLimb(limb, LimbSlot.FrontLeg);
            RefreshAssembly(context.controllers.oneLegTwoArm, context.moveStats.oneLegTwoArm, new TwoArmOneLegState());
            return true;
        }

        public override bool RemoveLimb(LimbSlot limb)
        {
            if (limb != LimbSlot.FrontArm && limb != LimbSlot.BackArm)
                return false;

            context.RemoveLimb(limb, true);
            RefreshAssembly(context.controllers.torsoOneArm, context.moveStats.torsoOneArm, new TorsoOneArmState());
            return true;
        }
    }
}

