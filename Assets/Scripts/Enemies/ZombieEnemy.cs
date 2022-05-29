using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class ZombieEnemy : MonoBehaviour, IDamageable
    {
        [SerializeField] float chaseSpeed;
        [SerializeField] float aggroRadius;
        [SerializeField] int damage;
        [SerializeField] float idleTimeOnHit;
        [SerializeField] SpriteRenderer sprite;

        private Transform player;
        private IAIBehavior activeBehavior;
        private float elapsed = 0f;


        void Start()
        {
            activeBehavior = new AIPatrol();
            activeBehavior.Initialize(transform);
        }

        void Update()
        {
            if (activeBehavior as AIIdle != null)
            {
                elapsed += Time.deltaTime;
                if (elapsed < idleTimeOnHit) 
                {
                    return;
                }
            }

            /*
            if (Vector2.Distance(player.position, transform.position) <= aggroRadius)
            {
                ChasePlayer();
            }
            */

            var pos = transform.position;
            activeBehavior.Update();
            sprite.flipX = pos.x < transform.position.x;
        }

        private void ChasePlayer()
        {
            if (activeBehavior as AIChase != null) return;

            var chase = new AIChase();
            chase.chaseSpeed = chaseSpeed;
            chase.player = player;
            chase.Initialize(transform);
            activeBehavior = chase;
            Debug.Log("Chasing the player!");
        }

        private void Idle()
        {
            elapsed = 0f;
            //activeBehavior = new AIIdle();
        }

        public void Damage(int dmg)
        {
            Debug.Log($"Killed {name}!");
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Player player))
            {
                player.Damage(damage);
                Idle();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, aggroRadius);
        }
    }

}

