using UnityEngine;
using System.Collections.Generic;

namespace FictionalOctoDoodle.Core
{
    public partial class LimbAssembly : MonoBehaviour
    {
        public int ArmCount { get; private set; }
        public BaseAssemblyMoveStats moveStats;
        public LimbAnimationControllers controllers;

        [SerializeField] LimbConnector torso;
        [SerializeField] LimbConnector neckArm;
        [SerializeField] LimbConnector neckLeg;
        [SerializeField] LimbConnector frontArm;
        [SerializeField] LimbConnector backArm;
        [SerializeField] LimbConnector frontLeg;
        [SerializeField] LimbConnector backLeg;

        [SerializeField] Collider2D legCollider;

        private Animator animator;
        private LimbAssemblyState state;
        private PlayerMovement playerMovement;


        private void Awake()
        {
            playerMovement = GetComponentInParent<PlayerMovement>();
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = controllers.skullOnly;
            legCollider.enabled = false;
            ChangeState(new SkullOnlyState());
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

            legCollider.enabled = frontLeg.limbData != null;

            ArmCount = 0;
            ArmCount += frontArm.limbData != null ? 1 : 0;
            ArmCount += backArm.limbData != null ? 1 : 0;
        }

        public void ChangeState(LimbAssemblyState newState)
        {
            state?.ExitState();
            state = newState;
            state.EnterState(this);
        }

        public void SetAnimationController(RuntimeAnimatorController controller)
        {
            animator.runtimeAnimatorController = null;
            animator.runtimeAnimatorController = controller;
            playerMovement.ReloadState();
        }

        public void RecalculateMoveStats(PlayerMoveStats stats)
        {
            var calculated = ScriptableObject.CreateInstance<PlayerMoveStats>();
            calculated.Copy(stats);
            var limbs = GetConnectedLimbs();
            for (int i = 0; i < limbs.Count; i++)
            {
                calculated.moveSpeed += limbs[i].moveSpeedModifier;
                calculated.climbSpeed += limbs[i].climbSpeedModifier;
                calculated.jumpForce += limbs[i].jumpForceModifier;
                calculated.swimSpeed += limbs[i].swimSpeedModifier;
            }
            playerMovement.baseStats = calculated;
        }

        public void OnAttackAnimFinished()
        {
            playerMovement.SetNewState(new IdleState());
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

        private List<LimbData> GetConnectedLimbs()
        {
            var list = new List<LimbData>();

            if (torso.limbData) list.Add(torso.limbData);
            if (frontArm.limbData) list.Add(frontArm.limbData);
            if (backArm.limbData) list.Add(backArm.limbData);
            if (frontLeg.limbData) list.Add(frontLeg.limbData);
            if (backLeg.limbData) list.Add(backLeg.limbData);

            return list;
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
}
