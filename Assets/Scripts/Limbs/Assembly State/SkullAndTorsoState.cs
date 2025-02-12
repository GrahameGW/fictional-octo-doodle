﻿using System.Linq;
using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class SkullAndTorsoState : LimbAssemblyState
    {
        public override bool AddLimb(LimbData limb)
        {
            if (!limb.Slots.Any(s => s != LimbSlot.Torso))
            {
                context.RemoveLimb(LimbSlot.Torso, true);
                context.AssembleLimb(limb, LimbSlot.Torso);
                RefreshAssembly(context.controllers.skullTorso, context.moveStats.skullTorso, this);
                return true;

            }

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

        public override bool RemoveLimb(LimbSlot limb, bool spawnCollectable)
        {
            if (limb != LimbSlot.Torso) return false;

            context.RemoveLimb(limb, spawnCollectable);
            RefreshAssembly(context.controllers.skullOnly, context.moveStats.skullOnly, new SkullOnlyState());
            return true;
        }
    }
}

