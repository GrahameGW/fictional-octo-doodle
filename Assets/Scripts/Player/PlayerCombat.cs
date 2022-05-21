using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class PlayerCombat : MonoBehaviour
    {
#if UNITY_EDITOR
        // just to make debug easier
        [SerializeField] int HP;
#endif

        [SerializeField] PlayerData data;
        [SerializeField] Animator animator;


        private void Awake()
        {
            data.HP = data.MaxHP;
            Debug.Log($"Loaded combat module. Player HP set to Max HP ({data})");
#if UNITY_EDITOR
            HP = data.HP;
#endif
        }

        public void Damage(int damage)
        {
            data.HP -= damage;
            animator.SetTrigger("damaged");
            if (data.HP <= 0)
            {
                data.HP = 0;
                Debug.Log("Died!");
                Time.timeScale = 0;
            }
#if UNITY_EDITOR
            HP = data.HP;
#endif
        }
    }
}

