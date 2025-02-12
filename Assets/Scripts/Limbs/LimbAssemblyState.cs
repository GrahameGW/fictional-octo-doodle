﻿using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public abstract class LimbAssemblyState
    {
        protected LimbAssembly context;
        
        public abstract bool AddLimb(LimbData limb);
        public abstract bool RemoveLimb(LimbSlot limb, bool spawnCollectable = true);

        public virtual void EnterState(LimbAssembly context) 
        {
            this.context = context;
        }
        public virtual void ExitState() { }

        public void RefreshAssembly(RuntimeAnimatorController ctrl, PlayerMoveStats stats, LimbAssemblyState state)
        {
            context.SetAnimationController(ctrl);
            context.RecalculateMoveStats(stats);
            context.ChangeState(state);
        }
    }
}
