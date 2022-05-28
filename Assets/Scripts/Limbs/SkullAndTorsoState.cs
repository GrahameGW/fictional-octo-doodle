using System.Linq;

namespace FictionalOctoDoodle.Core
{
    public class SkullAndTorsoState : LimbAssemblyState
    {
        public override bool AddLimb(LimbData limb)
        {
            if (!limb.Slots.Any(s => s != LimbSlot.Torso)) return false;
            var slot = limb.Slots.First(s => s != LimbSlot.Torso);

            context.AssembleLimb(limb, slot);
            LimbAssemblyState state = 
                slot == LimbSlot.BackArm || slot == LimbSlot.FrontArm ?
                new TorsoOneArmState() : new TorsoOneLegState();
            context.ChangeState(state);
            return true;
        }

        public override bool RemoveLimb(LimbSlot limb)
        {
            if (limb != LimbSlot.Torso) return false;

            context.RemoveLimb(limb);
            context.ChangeState(new SkullOnlyState());
            return true;
        }
    }
}

