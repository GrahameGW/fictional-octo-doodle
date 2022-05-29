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

        private IAIBehavior activeBehavior;
        private Animator animator;
        private bool chasing;
        private bool attacking;


        void Start()
        {
            activeBehavior = new AIPatrol();
            activeBehavior.Initialize(transform);
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            var pos = transform.position;
            activeBehavior.Update();
            FlipModels(pos.x > transform.position.x ? 0f : 180f);

            if (playerData == null || activeBehavior is AIIdle)
            {
                return;
            }

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
            activeBehavior = new AIIdle(attackTimer, ResumePatrol);
            animator.SetTrigger("attack");
            attacking = false;
        }

        private void ResumePatrol()
        {
            activeBehavior = new AIPatrol();
            activeBehavior.Initialize(transform);
            chasing = false;
            attacking = true;
        }

        public void Damage(int dmg)
        {
            animator.SetTrigger("die");
            Destroy(GetComponent<Rigidbody2D>());
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

