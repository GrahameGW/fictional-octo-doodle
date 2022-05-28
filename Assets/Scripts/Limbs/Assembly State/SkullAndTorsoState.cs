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
            PlayerMoveStats stats;

            if (slot == LimbSlot.BackArm || slot == LimbSlot.FrontArm)
            {
                state = new TorsoOneArmState();
                ctrl = context.controllers.torsoOneArm;
                stats = context.moveStats.torsoOneArm;
            }
            else
            {
                state = new TorsoOneLegState();
                ctrl = context.controllers.torsoOneLeg;
                stats = context.moveStats.torsoOneLeg;
            }

            context.AssembleLimb(limb, slot);
            RefreshAssembly(ctrl, stats, state);
            return true;
        }

        public override bool RemoveLimb(LimbSlot limb)
        {
            if (limb != LimbSlot.Torso) return false;

            context.RemoveLimb(limb);
            RefreshAssembly(context.controllers.skullOnly, context.moveStats.skullOnly, new SkullOnlyState());
            return true;
        }
    }
}

