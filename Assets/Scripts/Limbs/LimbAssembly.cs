using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace FictionalOctoDoodle.Core
{
    public class LimbAssembly : MonoBehaviour
    {
        [SerializeField] RuntimeAnimatorController defaultAnimController;
        
        [Serializable]
        public class LimbConnector
        {
            public LimbSlot slotId;
            public Vector2 position;
            public int orderInLayer;
            public LimbData limb;
            public RuntimeAnimatorController controller;
        }

        [SerializeField] LimbConnector torso;
        [SerializeField] LimbConnector rightArm;
        [SerializeField] LimbConnector leftArm;
        [SerializeField] LimbConnector rightLeg;
        [SerializeField] LimbConnector leftLeg;

        public Action OnLimbConfigChanged;

        private Animator animator;


        private void Awake()
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = defaultAnimController;
        }

        public void FlipModels(float degrees)
        {
                transform.eulerAngles = new Vector3(
                    transform.eulerAngles.x,
                    degrees,
                    transform.eulerAngles.z
                    );
        }
        public bool TryAddLimb(LimbData limb)
        {
            if (torso.limb == null && !limb.Slots.Any(s => s == LimbSlot.Torso))
            {
                return false;
            }

            if (torso.limb != null)
            {
                switch (limb.Slots[0])
                {
                    case LimbSlot.RightArm:
                        AssembleLimb(limb, rightArm);
                        return true;
                    case LimbSlot.Torso:
                        return false;
                    default: 
                        return false;
                }
            }


            if (limb.Slots.Any(s => s == LimbSlot.Torso))
            {
                AssembleLimb(limb, torso);
                return true;
            }

            return false;
        }

        private void AssembleLimb(LimbData limb, LimbConnector slot)
        {
            slot.limb = limb;
            var limbObjName = limb.Prefab.name;
            var obj = Instantiate(limb.Prefab, transform);
            obj.name = limbObjName;
            obj.transform.localPosition = slot.position;
            animator.runtimeAnimatorController = slot.controller;
            OnLimbConfigChanged?.Invoke();
            //characterModels.Add(obj.transform);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawSphere(torso.position, 0.25f);
            Gizmos.DrawSphere(rightArm.position, 0.25f);
            Gizmos.DrawSphere(leftArm.position, 0.25f);
            Gizmos.DrawSphere(rightLeg.position, 0.25f);
            Gizmos.DrawSphere(leftLeg.position, 0.25f);
        }
    }


}
