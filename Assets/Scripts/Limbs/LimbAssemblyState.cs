namespace FictionalOctoDoodle.Core
{
    public abstract class LimbAssemblyState
    {
        protected LimbAssembly context;
        
        public abstract bool AddLimb(LimbData limb);
        public abstract bool RemoveLimb(LimbSlot limb);

        public virtual void EnterState(LimbAssembly context) 
        {
            this.context = context;
        }
        public virtual void ExitState() { }
    }


}
