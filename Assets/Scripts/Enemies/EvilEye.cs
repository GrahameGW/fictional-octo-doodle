using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class EvilEye : MonoBehaviour
    {
        [SerializeField] Vector3[] patrolPath = default;
        [SerializeField] float patrolSpeed;
        [SerializeField] SpriteRenderer sprite;

        private IAIBehavior activeBehavior;

        // Start is called before the first frame update
        void Start()
        {
            var patrol = new AIPatrol();
            patrol.waypoints = patrolPath;
            patrol.patrolSpeed = patrolSpeed;
            patrol.Initialize(transform);
            activeBehavior = patrol;
        }

        // Update is called once per frame
        void Update()
        {
            var pos = transform.position;
            activeBehavior.Update();
            sprite.flipX = pos.x < transform.position.x;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < patrolPath.Length; i++)
            {
                Gizmos.DrawCube(patrolPath[i], Vector3.one * 0.2f);
            }
        }
    }
}

