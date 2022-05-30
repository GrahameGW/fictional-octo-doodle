using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FictionalOctoDoodle.Core
{
    public class Player : MonoBehaviour, IDamageable
    {
        public Action OnPlayerDeath;
        [SerializeField] PlayerData data;
        [SerializeField] SoundRandomizer sounds;
        [SerializeField] float invincibleOnHitTime;

        private Animator animator;
        private AudioSource audioSource;
        private PlayerMovement movement;
        private LimbAssembly assembly;

        private bool invincible = false;
        private float invincibleTimeElapsed;


        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            assembly = GetComponentInChildren<LimbAssembly>();
            audioSource = GetComponent<AudioSource>();
            movement = GetComponent<PlayerMovement>();
            data.HP = data.MaxHP;
            Debug.Log($"Loaded combat module. Player HP set to Max HP ({data})");

            if (data.activePlayerObject != null)
            {
                Destroy(data.activePlayerObject);
            }

            data.activePlayerObject = this;
            Debug.Log("New player spawned at " + transform.position.ToString());
        }

        private void Update()
        {
            if (invincible)
            {
                invincibleTimeElapsed += Time.deltaTime;
                invincible = invincibleTimeElapsed < invincibleOnHitTime;
            }
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
            if (damage == int.MaxValue)
            {
                animator.SetTrigger("damaged");
                assembly.LoseAllLimbs();
                invincibleOnHitTime = int.MaxValue;
                Destroy(movement);
                StartCoroutine(DeathRoutine());
                return;
            }
            else if (invincible || movement.ActiveState is AttackingState)
            {
                return;
            }

            assembly.LoseRandomLimb(out bool wasSkull);
            animator.SetBool("moving", false);
            animator.SetTrigger("damaged");
            invincible = true;
            invincibleTimeElapsed = 0f;

            if (wasSkull)
            {
                invincibleOnHitTime = int.MaxValue;
                Destroy(movement);
                StartCoroutine(DeathRoutine());
            }
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

        private IEnumerator DeathRoutine()
        {
            var randVec = Quaternion.Euler(0f, 0f, Random.Range(25f, 155f)) * Vector2.right *
                Random.Range(5f, 10f);

            GetComponent<Rigidbody2D>().AddForce(randVec);
            yield return new WaitForSeconds(3f);
            Debug.Log("You died!");
            Destroy(gameObject);
        }

    }
}

