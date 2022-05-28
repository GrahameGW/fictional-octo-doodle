using System.Linq;

namespace FictionalOctoDoodle.Core
{
    public class OneArmTwoLegState : LimbAssemblyState
    {
        public override bool AddLimb(LimbData limb)
        {
            if (!limb.Slots.Contains(LimbSlot.BackArm)) return false;

            context.AssembleLimb(limb, LimbSlot.BackArm);
            context.ChangeState(new FullyAssembledState());
            return true;
        }

        public override bool RemoveLimb(LimbSlot limb)
        {
            if (limb == LimbSlot.Torso || limb == LimbSlot.BackArm) return false;

            LimbAssemblyState state = limb == LimbSlot.FrontArm ?
                new TorsoTwoLegState() : new OneArmOneLegState();

            context.RemoveLimb(limb);
            context.ChangeState(state);
            return true;
        }
    }
}

