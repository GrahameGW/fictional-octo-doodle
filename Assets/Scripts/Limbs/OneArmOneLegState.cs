using System.Linq;

namespace FictionalOctoDoodle.Core
{
    public class OneArmOneLegState : LimbAssemblyState
    {
        public override bool AddLimb(LimbData limb)
        {
            if (!limb.Slots.Any(s => s == LimbSlot.BackArm || s == LimbSlot.BackLeg)) return false;
            var slot = limb.Slots.First(s => s == LimbSlot.BackArm || s == LimbSlot.BackLeg);

            context.AssembleLimb(limb, slot);
            LimbAssemblyState state = slot == LimbSlot.BackArm ?
                new TwoArmOneLegState() : new OneArmTwoLegState();

            context.ChangeState(state);
            return true;
        }

        public override bool RemoveLimb(LimbSlot limb)
        {
            if (limb == LimbSlot.Torso) return false;
            
            context.RemoveLimb(limb);
            LimbAssemblyState state = limb == LimbSlot.BackArm || limb == LimbSlot.FrontArm ?
                new TorsoOneLegState() : new TorsoOneArmState();
            context.ChangeState(state);
            return true;
        }
    }
}

