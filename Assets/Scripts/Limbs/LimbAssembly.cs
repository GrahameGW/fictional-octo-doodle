using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

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
            playerMovement.distanceToGround = 2f;
        }

        public bool TryAddLimb(LimbData limb)
        {
            return state.AddLimb(limb);
        }

        public void RemoveLimb(LimbSlot limb, bool spawnCollectable)
        {
            var slot = GetSlotById(limb);
            var removed = Instantiate(slot.limbData.Token, transform.position + new Vector3(0f, 3f, 0f), Quaternion.identity)
                .GetComponent<LimbCollectable>();
            removed.Persists = spawnCollectable;
            slot.ClearLoadedLimb();

            if (limb == LimbSlot.FrontLeg)
            {
                legCollider.enabled = false;
            }

            if (slot.slotId == LimbSlot.Torso || slot.slotId == LimbSlot.FrontLeg)
            {
                playerMovement.distanceToGround -= 3f;
            }
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

            if (slot.slotId == LimbSlot.Torso || slot.slotId == LimbSlot.FrontLeg)
            {
                transform.parent.Translate(Vector2.up * 2f);
                playerMovement.distanceToGround += 3f;
            }
        }

        public void ChangeState(LimbAssemblyState newState)
        {
            state?.ExitState();
            state = newState;
            state.EnterState(this);
        }

        public void SetAnimationController(RuntimeAnimatorController controller)
        {
            var acParams = animator.parameters;
            animator.runtimeAnimatorController = null;
            animator.runtimeAnimatorController = controller;

            StartCoroutine(RebindAnimator(acParams));
        }

        public void LoseAllLimbs()
        {
            if (backLeg.limbData != null) state.RemoveLimb(LimbSlot.BackLeg, false);
            if (backArm.limbData != null) state.RemoveLimb(LimbSlot.BackArm, false);
            if (frontLeg.limbData != null) state.RemoveLimb(LimbSlot.FrontLeg, false);
            if (frontArm.limbData != null) state.RemoveLimb(LimbSlot.FrontArm, false);
            if (torso.limbData != null) state.RemoveLimb(LimbSlot.Torso, false);
        }

        public void LoseRandomLimb(out bool limbWasSkull)
        {
            var limbs = GetFilledLimbSlots();

            limbWasSkull = limbs.Count == 0;
            if (limbWasSkull) return;

            if (limbs.Count == 1)
            {
                state.RemoveLimb(LimbSlot.Torso, false);
                return;
            }

            var back = limbs.FirstOrDefault(l => l == LimbSlot.BackArm || l == LimbSlot.BackLeg);
            if (back != default) // default limbslot is FrontArm
            {
                state.RemoveLimb(back, false);
            }
            else
            {
                state.RemoveLimb(limbs.First(l => l == LimbSlot.FrontArm || l == LimbSlot.FrontLeg), false);
            }
        }
        private IEnumerator RebindAnimator(AnimatorControllerParameter[] acParams)
        {
            yield return null;
            animator.Rebind();
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

        private List<LimbSlot> GetFilledLimbSlots()
        {
            var list = new List<LimbSlot>();

            if (torso.limbData) list.Add(LimbSlot.Torso);
            if (frontArm.limbData) list.Add(LimbSlot.FrontArm);
            if (backArm.limbData) list.Add(LimbSlot.BackArm);
            if (frontLeg.limbData) list.Add(LimbSlot.FrontLeg);
            if (backLeg.limbData) list.Add(LimbSlot.BackLeg);

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
