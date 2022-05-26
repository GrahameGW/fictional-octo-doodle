using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace FictionalOctoDoodle.Core
{
    public class LimbAssembly : MonoBehaviour
    {
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
        private List<Transform> characterModels;


        private void Awake()
        {
            animator = GetComponent<Animator>();
            characterModels = new List<Transform>() { transform.GetChild(0) }; // get skull;
        }

        public void FlipModels(float degrees)
        {
            for (int i = 0; i < characterModels.Count; i++)
            {
                characterModels[i].eulerAngles = new Vector3(
                    characterModels[i].eulerAngles.x,
                    degrees,
                    characterModels[i].eulerAngles.z
                    );
            }
        }
        public void AddLimb(LimbData limb)
        {
            if (limb.Slots.Any(s => s == LimbSlot.Torso) && torso.limb == null)
            {
                torso.limb = limb;
                var limbObjName = limb.Prefab.name;
                var obj = Instantiate(limb.Prefab, transform);
                obj.name = limbObjName;
                obj.transform.localPosition = torso.position;
                animator.runtimeAnimatorController = torso.controller;
                OnLimbConfigChanged?.Invoke();
                characterModels.Add(obj.transform);
            }
        }

        private void OnDrawGizmosSelected()
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
