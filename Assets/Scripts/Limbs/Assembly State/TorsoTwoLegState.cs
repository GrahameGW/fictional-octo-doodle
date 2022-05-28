using System.Linq;

namespace FictionalOctoDoodle.Core
{
    public class TorsoTwoLegState : LimbAssemblyState
    {
        public override bool AddLimb(LimbData limb)
        {
            if (!limb.Slots.Contains(LimbSlot.FrontArm)) return false;

            context.AssembleLimb(limb, LimbSlot.FrontArm);
            RefreshAssembly(context.controllers.twoLegOneArm, context.moveStats.twoLegOneArm, new OneArmTwoLegState());

            return true;
        }

        public override bool RemoveLimb(LimbSlot limb)
        {
            if (limb != LimbSlot.FrontLeg || limb != LimbSlot.BackLeg)
            {
                return false;
            }

            context.RemoveLimb(limb);
            RefreshAssembly(context.controllers.torsoOneLeg, context.moveStats.torsoOneLeg, new TorsoOneLegState());
            return true;
        }
    }
}

