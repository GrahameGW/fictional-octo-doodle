using UnityEngine;
using System;
using System.Linq;


namespace FictionalOctoDoodle.Core
{
    public partial class LimbAssembly : MonoBehaviour
    {
        public Action OnLimbConfigChanged;

        [SerializeField] LimbConnector torso;
        [SerializeField] LimbConnector neckArm;
        [SerializeField] LimbConnector neckLeg;
        [SerializeField] LimbConnector frontArm;
        [SerializeField] LimbConnector backArm;
        [SerializeField] LimbConnector frontLeg;
        [SerializeField] LimbConnector backLeg;

        [SerializeField] LimbAnimationControllers controllers;
        [SerializeField] Collider2D legCollider;

        private Animator animator;
        private LimbAssemblyState state;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = SetAnimationController();
            legCollider.enabled = false;
            ChangeState(new SkullOnlyState());
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
            return state.AddLimb(limb);
        }

        public void RemoveLimb(LimbSlot limb)
        {
            GetSlotById(limb).ClearLoadedLimb();
        }

        public void AssembleLimb(LimbData limb, LimbSlot slotId)
        {
            var slot = GetSlotById(slotId);
            
            slot.ClearLoadedLimb();
            slot.limbData = limb;

            var obj = Instantiate(limb.Prefab, transform);
            obj.name = SetLimbName(slot.slotId);
            if (slot.slotId == LimbSlot.BackArm || slot.slotId == LimbSlot.BackLeg)
            {
                LabelBackLimbs(obj.transform);
            }
            obj.transform.localPosition = slot.position;
            obj.GetComponent<SpriteRenderer>().sortingOrder = slot.orderInLayer;
            slot.limbObj = obj;

            var newController = SetAnimationController();
            animator.runtimeAnimatorController = null;
            animator.runtimeAnimatorController = newController;
            OnLimbConfigChanged?.Invoke();

            legCollider.enabled = frontLeg.limbData != null;
        }

        public void ChangeState(LimbAssemblyState newState)
        {
            state?.ExitState();
            state = newState;
            state.EnterState(this);
        }
        private RuntimeAnimatorController SetAnimationController()
        {
            if (torso.limbData == null)
            {
                if (neckArm.limbData == null && neckLeg.limbData == null)
                {
                    return controllers.skullOnly;
                }

                return neckArm.limbData != null ?
                    controllers.skullNeckArm :
                    controllers.skullNeckLeg;
            }

            if (backArm.limbData != null)
            {
                if (backLeg.limbData != null)
                {
                    return controllers.fullBody;
                }

                return frontLeg.limbData != null ?
                    controllers.oneLegTwoArm :
                    controllers.torsoTwoArm;
            }

            if (frontArm.limbData != null)
            {
                if (backLeg.limbData != null)
                {
                    return controllers.twoLegOneArm;
                }

                return frontLeg.limbData != null ?
                    controllers.oneLegOneArm :
                    controllers.torsoOneArm;
            }

            if (backLeg.limbData != null)
            {
                return controllers.torsoTwoLeg;
            }

            return frontLeg.limbData != null ?
                controllers.torsoOneLeg :
                controllers.skullTorso;
        }

        private void LabelBackLimbs(Transform limb)
        {
            if (limb.childCount == 0) return;
            foreach (Transform child in limb)
            {
                child.name += "Back"; // so the animator works later 
                LabelBackLimbs(child); // recursion muthafucka
            }
        }

        private string SetLimbName(LimbSlot slot)
        {
            return slot switch
            {
                LimbSlot.Torso => "SkeletonTorso",
                LimbSlot.FrontLeg => "SkeletonLeg",
                LimbSlot.BackLeg => "SkeletonLegBack",
                LimbSlot.BackArm => "SkeletonArmBack",
                LimbSlot.FrontArm => "SkeletonArm",
                _ => "No Slot Found",
            };
        }

        private LimbConnector GetSlotById(LimbSlot slotId)
        {
            return slotId switch
            {
                LimbSlot.Torso => torso,
                LimbSlot.FrontLeg => frontLeg,
                LimbSlot.BackLeg => backLeg,
                LimbSlot.BackArm => backArm,
                LimbSlot.FrontArm => frontArm,
                _ => null
            };
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawSphere((Vector2)transform.position + torso.position, 0.25f);
            Gizmos.DrawSphere((Vector2)transform.position + frontArm.position, 0.25f);
            Gizmos.DrawSphere((Vector2)transform.position + backArm.position, 0.25f);
            Gizmos.DrawSphere((Vector2)transform.position + frontLeg.position, 0.25f);
            Gizmos.DrawSphere((Vector2)transform.position + backLeg.position, 0.25f);
        }
    }


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
