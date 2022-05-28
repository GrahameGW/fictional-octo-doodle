using System.Linq;

namespace FictionalOctoDoodle.Core
{
    public class TorsoOneLegState : LimbAssemblyState
    {
        public override bool AddLimb(LimbData limb)
        {
            if (!limb.Slots.Any(s => s != LimbSlot.Torso || s == LimbSlot.FrontLeg)) return false;
            var slot = limb.Slots.First(s => s == LimbSlot.BackLeg || s == LimbSlot.FrontArm);

            context.AssembleLimb(limb, slot);
            LimbAssemblyState state = slot == LimbSlot.BackLeg ?
                new TorsoTwoLegState() : new OneArmOneLegState();
            context.ChangeState(state);
            return true;
        }

        public override bool RemoveLimb(LimbSlot limb)
        {
            if (limb != LimbSlot.FrontLeg) return false;

            context.RemoveLimb(limb);
            context.ChangeState(new SkullAndTorsoState());
            return true;
        }
    }
}

