using System.Linq;
using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class OneArmTwoLegState : LimbAssemblyState
    {
        public override bool AddLimb(LimbData limb)
        {
            if (!limb.Slots.Contains(LimbSlot.BackArm))
            {
                var slot = limb.Slots[Random.Range(0, limb.Slots.Length)];

                context.RemoveLimb(slot, true);
                context.AssembleLimb(limb, slot);
                RefreshAssembly(context.controllers.twoLegOneArm, context.moveStats.twoLegOneArm, this);
                return true;
            }

            context.AssembleLimb(limb, LimbSlot.BackArm);
            RefreshAssembly(context.controllers.fullBody, context.moveStats.fullBody, new FullyAssembledState());
            return true;
        }

        public override bool RemoveLimb(LimbSlot limb, bool spawnCollectable)
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

            context.RemoveLimb(limb, spawnCollectable);
            RefreshAssembly(ctrl, stats, state);
            return true;
        }
    }
}

