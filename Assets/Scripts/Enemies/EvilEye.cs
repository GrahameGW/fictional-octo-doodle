using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class EvilEye : MonoBehaviour
    {
        [SerializeField] Vector3[] patrolPath = default;
        [SerializeField] float patrolSpeed;
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
            var patrol = new AIPatrol();
            patrol.waypoints = patrolPath;
            patrol.patrolSpeed = patrolSpeed;
            patrol.Initialize(transform);
            activeBehavior = patrol;

            player = FindObjectOfType<PlayerCombat>().transform;
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

            if (Vector2.Distance(player.position, transform.position) <= aggroRadius)
            {
                ChasePlayer();
            }

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
            activeBehavior = new AIIdle();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out PlayerCombat player))
            {
                player.Damage(damage);
                Idle();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < patrolPath.Length; i++)
            {
                Gizmos.DrawCube(patrolPath[i], Vector3.one * 0.2f);
            }

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, aggroRadius);
        }


    }
}

