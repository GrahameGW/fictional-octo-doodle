using System;
using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class Player : MonoBehaviour, IDamageable
    {
#if UNITY_EDITOR
        // just to make debug easier
        [DisplayOnly]
        [SerializeField] int HP;
#endif
        [SerializeField] PlayerData data;
        [SerializeField] Animator animator;

        [Serializable]
        public class PlayerLimbs
        {
            public LimbData torso;
            public LimbData rightArm;
            public LimbData leftArm;
            public LimbData rightLeg;
            public LimbData leftLeg;
        }

        [SerializeField] PlayerLimbs limbs;

        private void Awake()
        {
            data.HP = data.MaxHP;
            Debug.Log($"Loaded combat module. Player HP set to Max HP ({data})");
#if UNITY_EDITOR
            HP = data.HP;
#endif
        }

        public void AddLimb(LimbData limb)
        {
            for (int i = 0; i < limb.Slots.Length; i++)
            {
                var slot = limb.Slots[i];

                if (slot == LimbSlot.RightArm && limbs.rightArm == null)
                {
                    limbs.rightArm = limb;
                    return;
                } 

                if (slot == LimbSlot.LeftArm && limbs.leftArm == null)
                {
                    limbs.leftArm = limb;
                    return;
                }

                if (slot == LimbSlot.Torso && limbs.torso == null)
                {
                    limbs.torso = limb;
                    return;
                }
                
                if (slot == LimbSlot.RightLeg && limbs.rightLeg == null)
                {
                    limbs.rightLeg = limb;
                    return;
                }

                if (slot == LimbSlot.LeftLeg && limbs.leftLeg == null)
                {
                    limbs.leftLeg = limb;
                    return;
                }
            }
        }

        public void Damage(int damage)
        {
            data.HP -= damage;
            animator.SetTrigger("damaged");
            if (data.HP <= 0)
            {
                data.HP = 0;
                Debug.Log("Died!");
                OnPlayerDeath?.Invoke();
                Destroy(gameObject);
            }
#if UNITY_EDITOR
            HP = data.HP;
#endif
        }

        public Action OnPlayerDeath;

    }
}

