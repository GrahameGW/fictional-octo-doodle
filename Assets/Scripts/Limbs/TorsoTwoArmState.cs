using System.Linq;

namespace FictionalOctoDoodle.Core
{
    public class TorsoTwoArmState : LimbAssemblyState
    {
        public override bool AddLimb(LimbData limb)
        {
            if (!limb.Slots.Contains(LimbSlot.FrontLeg)) return false;

            context.AssembleLimb(limb, LimbSlot.FrontLeg);
            context.ChangeState(new TwoArmOneLegState());
            return true;
        }

        public override bool RemoveLimb(LimbSlot limb)
        {
            if (limb != LimbSlot.FrontArm && limb != LimbSlot.BackArm)
                return false;

            context.RemoveLimb(limb);
            context.ChangeState(new TorsoOneArmState());
            return true;
        }
    }
}

