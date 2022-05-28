using System.Linq;

namespace FictionalOctoDoodle.Core
{
    public class TorsoOneArmState : LimbAssemblyState
    {
        public override bool AddLimb(LimbData limb)
        {
            if (!limb.Slots.Any(s => s != LimbSlot.Torso || s != LimbSlot.FrontArm)) return false;
            var slot = limb.Slots.First(s => s == LimbSlot.FrontLeg || s == LimbSlot.BackArm);

            context.AssembleLimb(limb, slot);
            LimbAssemblyState state = slot == LimbSlot.BackArm ?
                new TorsoTwoArmState() : new OneArmOneLegState();

            context.ChangeState(state);
            return true;
        }

        public override bool RemoveLimb(LimbSlot limb)
        {
            if (limb != LimbSlot.FrontArm) return false;

            context.RemoveLimb(limb);
            context.ChangeState(new SkullAndTorsoState());
            return true;
        }
    }
}

