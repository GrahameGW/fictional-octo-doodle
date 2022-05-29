using System;
using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class Player : MonoBehaviour, IDamageable
    {
        public Action OnPlayerDeath;
        [SerializeField] PlayerData data;
        
        private Animator animator;
        private PlayerMovement movement;


        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            movement = GetComponent<PlayerMovement>();
            data.HP = data.MaxHP;
            Debug.Log($"Loaded combat module. Player HP set to Max HP ({data})");
#if UNITY_EDITOR
            HP = data.HP;
#endif

            if (data.activePlayerObject != null)
            {
                Destroy(data.activePlayerObject);
            }

            data.activePlayerObject = this;
            Debug.Log("New player spawned at " + transform.position.ToString());
        }

        public void Damage(int damage)
        {
            if (movement.ActiveState is AttackingState) return;

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


#if UNITY_EDITOR
        // just to make debug easier
        [DisplayOnly]
        [SerializeField] int HP;
#endif

    }
}

