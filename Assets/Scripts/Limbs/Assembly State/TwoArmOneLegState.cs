using System.Linq;
using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class TwoArmOneLegState : LimbAssemblyState
    {
        public override bool AddLimb(LimbData limb)
        {
            if (!limb.Slots.Contains(LimbSlot.BackLeg))
            {
                var slot = limb.Slots[Random.Range(0, limb.Slots.Length)];

                context.RemoveLimb(slot, true);
                context.AssembleLimb(limb, slot);
                RefreshAssembly(context.controllers.oneLegTwoArm, context.moveStats.oneLegTwoArm, this);
                return true;
            }

            context.AssembleLimb(limb, LimbSlot.BackLeg);
            RefreshAssembly(context.controllers.fullBody, context.moveStats.fullBody, new FullyAssembledState());
            return true;
        }

        public override bool RemoveLimb(LimbSlot limb, bool spawnCollectable)
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

            context.RemoveLimb(limb, spawnCollectable);
            RefreshAssembly(ctrl, stats, state);
            return true;
        }
    }
}

