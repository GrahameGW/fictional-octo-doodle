using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class ZombieEnemy : MonoBehaviour, IDamageable
    {
        [SerializeField] float chaseSpeed;
        [SerializeField] float aggroRadius;
        [SerializeField] float attackRadius;
        [SerializeField] int touchDamage;
        [SerializeField] int attackDamage;
        [SerializeField] float attackTimer;
        [SerializeField] float idleTimeOnHit;

        [SerializeField] Transform modelRoot;
        [SerializeField] PlayerData playerData;
        [SerializeField] SoundRandomizer sounds;
        [SerializeField] SoundRandomizer hitBySounds;

        private IAIBehavior activeBehavior;
        private Animator animator;
        private AudioSource audioSource;
        private bool chasing;
        private bool attacking;


        void Start()
        {
            activeBehavior = new AIPatrol();
            activeBehavior.Initialize(transform);
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
        }

        void Update()
        {
            var pos = transform.position;
            activeBehavior.Update();

            if (playerData.activePlayerObject == null || activeBehavior is AIIdle || attacking)
            {
                return;
            }
            
            FlipModels(pos.x > transform.position.x ? 0f : 180f);

            float dist = Vector3.Distance(transform.position, playerData.activePlayerObject.transform.position);
            if (attackRadius >= dist)
            {
                AttackPlayer();
            }
            else if (!chasing && aggroRadius >= dist)
            {
                ChasePlayer();
            }
            else if (chasing && aggroRadius < dist)
            {
                ResumePatrol();
            }
        }

        private void FlipModels(float degrees)
        {
            modelRoot.eulerAngles = new Vector3(
                transform.eulerAngles.x,
                degrees,
                transform.eulerAngles.z
                );
        }
        private void ChasePlayer()
        {
            if (activeBehavior as AIChase != null) return;

            var chase = new AIChase();
            chase.chaseSpeed = chaseSpeed;
            chase.player = playerData.activePlayerObject.transform;
            chase.Initialize(transform);
            activeBehavior = chase;
            Debug.Log("Chasing the player!");
        }
        private void AttackPlayer()
        {
            activeBehavior = new AIIdle(attackTimer, chasing ? ChasePlayer : ResumePatrol);
            FlipModels(playerData.activePlayerObject.transform.position.x < transform.position.x ? 0f : 180f);
            animator.SetTrigger("attack");
            audioSource.PlayOneShot(sounds.GetClip());
            attacking = true;
        }

        private void ResumePatrol()
        {
            activeBehavior = new AIPatrol();
            activeBehavior.Initialize(transform);
            chasing = false;
            attacking = false;
            animator.ResetTrigger("attack");
        }

        public void Damage(int dmg)
        {
            animator.SetTrigger("die");
            GetComponent<Rigidbody2D>().simulated = false;
            activeBehavior = new AIIdle(float.PositiveInfinity, null);
            audioSource.PlayOneShot(hitBySounds.GetClip());
            foreach (Collider2D c in GetComponentsInChildren<Collider2D>())
            {
                c.enabled = false;
            }
        }

        public void OnDeathAnimComplete()
        {
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Player player))
            {
                player.Damage(attacking ? attackDamage : touchDamage);
                activeBehavior = new AIIdle(idleTimeOnHit, ResumePatrol);
                chasing = false;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, aggroRadius);
            Gizmos.DrawWireSphere(transform.position, attackRadius);
        }
    }
}

