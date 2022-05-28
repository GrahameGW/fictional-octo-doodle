using System.Linq;

namespace FictionalOctoDoodle.Core
{
    public class TorsoTwoArmState : LimbAssemblyState
    {
        public override bool AddLimb(LimbData limb)
        {
            if (!limb.Slots.Contains(LimbSlot.FrontLeg)) return false;

            context.AssembleLimb(limb, LimbSlot.FrontLeg);
            RefreshAssembly(context.controllers.oneLegTwoArm, context.moveStats.oneLegTwoArm, new TwoArmOneLegState());
            return true;
        }

        public override bool RemoveLimb(LimbSlot limb)
        {
            if (limb != LimbSlot.FrontArm && limb != LimbSlot.BackArm)
                return false;

            context.RemoveLimb(limb);
            RefreshAssembly(context.controllers.torsoOneArm, context.moveStats.torsoOneArm, new TorsoOneArmState());
            return true;
        }
    }
}

