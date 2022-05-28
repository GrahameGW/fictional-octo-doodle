using UnityEngine;
using System;


namespace FictionalOctoDoodle.Core
{
    public partial class LimbAssembly
    {
        [Serializable]
        public class LimbAnimationControllers
        {
            public RuntimeAnimatorController skullOnly;
            public RuntimeAnimatorController skullTorso;
            public RuntimeAnimatorController skullNeckArm;
            public RuntimeAnimatorController skullNeckLeg;
            public RuntimeAnimatorController torsoOneArm;
            public RuntimeAnimatorController torsoTwoArm;
            public RuntimeAnimatorController torsoOneLeg;
            public RuntimeAnimatorController torsoTwoLeg;
            public RuntimeAnimatorController oneLegOneArm;
            public RuntimeAnimatorController oneLegTwoArm;
            public RuntimeAnimatorController twoLegOneArm;
            public RuntimeAnimatorController fullBody;
        }

        [Serializable]
        public class BaseAssemblyMoveStats
        {            
            public PlayerMoveStats skullOnly;
            public PlayerMoveStats skullTorso;
            public PlayerMoveStats skullNeckArm;
            public PlayerMoveStats skullNeckLeg;
            public PlayerMoveStats torsoOneArm;
            public PlayerMoveStats torsoTwoArm;
            public PlayerMoveStats torsoOneLeg;
            public PlayerMoveStats torsoTwoLeg;
            public PlayerMoveStats oneLegOneArm;
            public PlayerMoveStats oneLegTwoArm;
            public PlayerMoveStats twoLegOneArm;
            public PlayerMoveStats fullBody;
        }

    }
}
