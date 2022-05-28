namespace FictionalOctoDoodle.Core
{
    public class FullyAssembledState : LimbAssemblyState
    {
        public override bool AddLimb(LimbData limb)
        {
            return false;
        }

        public override bool RemoveLimb(LimbSlot limb)
        {
            if (limb == LimbSlot.Torso) return false;

            LimbAssemblyState state = limb == LimbSlot.FrontArm || limb == LimbSlot.BackArm ?
                new OneArmTwoLegState() : new TwoArmOneLegState();

            context.RemoveLimb(limb);
            context.ChangeState(state);
            return true;
        }
    }
}

