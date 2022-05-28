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
            context.SetAnimationController(context.controllers.oneLegTwoArm);
            return true;
        }

        public override bool RemoveLimb(LimbSlot limb)
        {
            if (limb != LimbSlot.FrontArm && limb != LimbSlot.BackArm)
                return false;

            context.RemoveLimb(limb);
            context.ChangeState(new TorsoOneArmState());
            context.SetAnimationController(context.controllers.torsoOneArm);
            return true;
        }
    }
}

