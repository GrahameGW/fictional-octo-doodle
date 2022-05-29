using System;
using System.Collections;
using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class Player : MonoBehaviour, IDamageable
    {
        public Action OnPlayerDeath;
        [SerializeField] PlayerData data;
        [SerializeField] SoundRandomizer sounds;
        
        private Animator animator;
        private AudioSource audioSource;
        private PlayerMovement movement;
        private LimbAssembly assembly;


        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            assembly = GetComponentInChildren<LimbAssembly>();
            audioSource = GetComponent<AudioSource>();
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

        private void OnEnable()
        {
            movement.OnStateChanged += MoveStateChangedHandler;
        }
        private void OnDisable()
        {
            movement.OnStateChanged -= MoveStateChangedHandler;
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

        private void MoveStateChangedHandler()
        {
            if (!(movement.ActiveState is AttackingState)) return;
            if (assembly.ArmCount == 2)
            {
                StartCoroutine(OneTwoPunchSounds());
            }
            else
            {
                audioSource.PlayOneShot(sounds.GetClip());
            }
        }

        private IEnumerator OneTwoPunchSounds()
        {
            audioSource.PlayOneShot(sounds.GetClip());
            yield return new WaitForSeconds(0.1f);
            audioSource.Stop();
            audioSource.PlayOneShot(sounds.GetClip());
        }


#if UNITY_EDITOR
        // just to make debug easier
        [DisplayOnly]
        [SerializeField] int HP;
#endif

    }
}

