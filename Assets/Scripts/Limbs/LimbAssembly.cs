using UnityEngine;
using System;

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
        }

        [SerializeField] LimbConnector torso;
        [SerializeField] LimbConnector rightArm;
        [SerializeField] LimbConnector leftArm;
        [SerializeField] LimbConnector rightLeg;
        [SerializeField] LimbConnector leftLeg;

        public Action OnLimbConfigChanged;


        public void AddLimb(LimbData limb)
        {
            /*
            var connection = new LimbConnector()
            {
                slotId = limb.Slots[0];

            }
            */
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
