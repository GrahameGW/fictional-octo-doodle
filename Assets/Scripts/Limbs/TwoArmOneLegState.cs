using System.Linq;

namespace FictionalOctoDoodle.Core
{
    public class TwoArmOneLegState : LimbAssemblyState
    {
        public override bool AddLimb(LimbData limb)
        {
            if (!limb.Slots.Contains(LimbSlot.BackLeg)) return false;

            context.AssembleLimb(limb, LimbSlot.BackLeg);
            context.ChangeState(new FullyAssembledState());
            return true;
        }

        public override bool RemoveLimb(LimbSlot limb)
        {
            if (limb == LimbSlot.Torso || limb == LimbSlot.BackLeg) return false;

            context.RemoveLimb(limb);
            LimbAssemblyState state = limb == LimbSlot.FrontLeg ?
                new TorsoTwoArmState() : new OneArmOneLegState();
            context.ChangeState(state);
            return true;
        }
    }
}

