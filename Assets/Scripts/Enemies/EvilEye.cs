using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class EvilEye : MonoBehaviour
    {
        [SerializeField] Vector3[] patrolPath = default;
        [SerializeField] float patrolSpeed;
        [SerializeField] float chaseSpeed;
        [SerializeField] float aggroRadius;
        [SerializeField] SpriteRenderer sprite;

        private Transform player;
        private IAIBehavior activeBehavior;

        // Start is called before the first frame update
        void Start()
        {
            var patrol = new AIPatrol();
            patrol.waypoints = patrolPath;
            patrol.patrolSpeed = patrolSpeed;
            patrol.Initialize(transform);
            activeBehavior = patrol;

            player = FindObjectOfType<PlayerCombat>().transform;
        }

        // Update is called once per frame
        void Update()
        {
            var pos = transform.position;
            activeBehavior.Update();
            sprite.flipX = pos.x < transform.position.x;

            if (Vector2.Distance(player.position, transform.position) <= aggroRadius)
            {
                ChasePlayer();
            }
        }

        private void ChasePlayer()
        {
            var chase = new AIChase();
            chase.chaseSpeed = chaseSpeed;
            chase.player = player;
            chase.Initialize(transform);
            activeBehavior = chase;
            Debug.Log("Chasing the player!");
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

