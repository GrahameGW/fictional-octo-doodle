using System.Linq;
using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class TorsoOneArmState : LimbAssemblyState
    {
        public override bool AddLimb(LimbData limb)
        {
            LimbSlot slot;

            if (!limb.Slots.Any(s => s != LimbSlot.Torso && s != LimbSlot.FrontArm))
            {
                slot = limb.Slots[Random.Range(0, limb.Slots.Length)];

                context.RemoveLimb(slot, true);
                context.AssembleLimb(limb, slot);
                RefreshAssembly(context.controllers.torsoOneArm, context.moveStats.torsoOneArm, this);
                return true;
            }

            slot = limb.Slots.First(s => s == LimbSlot.FrontLeg || s == LimbSlot.BackArm);

            LimbAssemblyState state;
            RuntimeAnimatorController ctrl;

            if (slot == LimbSlot.BackArm)
            {
                state = new TorsoTwoArmState();
                ctrl = context.controllers.torsoTwoArm;
            }
            else
            {
                state = new OneArmOneLegState();
                ctrl = context.controllers.oneLegOneArm;
            }

            context.AssembleLimb(limb, slot);
            context.ChangeState(state);
            context.SetAnimationController(ctrl);
            return true;
        }

        public override bool RemoveLimb(LimbSlot limb, bool spawnCollectable)
        {
            if (limb != LimbSlot.FrontArm) return false;

            context.RemoveLimb(limb, spawnCollectable);
            context.ChangeState(new SkullAndTorsoState());
            context.SetAnimationController(context.controllers.skullTorso);
            return true;
        }
    }
}

