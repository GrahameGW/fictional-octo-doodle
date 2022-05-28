using System.Linq;
using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class SkullOnlyState : LimbAssemblyState
    {
        public override bool AddLimb(LimbData limb)
        {
            if (limb.Slots.Contains(LimbSlot.Torso))
            {
                context.AssembleLimb(limb, LimbSlot.Torso);
                context.ChangeState(new SkullAndTorsoState());
                return true;
            }

            // leg neck and arm neck return true;
            return false;
        }

        public override bool RemoveLimb(LimbSlot limb)
        {
            Debug.Log("Skull is base state, can't remove any limbs!");
            return false;
        }
    }
}

