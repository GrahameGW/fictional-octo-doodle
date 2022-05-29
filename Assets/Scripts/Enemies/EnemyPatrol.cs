using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class EnemyPatrol : MonoBehaviour
    {
        [field: SerializeField] 
        public Vector3[] Path { get; private set; }
        [field: SerializeField]
        public float Speed { get; private set; }
        [field: SerializeField]
        public bool Randomize { get; private set; }


        private void Start()
        {
            if (Path == null || Path.Length == 0)
            {
                Path = new Vector3[] { transform.position };
            }
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            if (Path?.Length == 0) return;

            for (int i = 0; i < Path.Length; i++)
            {
                Gizmos.DrawCube(Path[i], Vector3.one * 0.2f);
            }

            if (!Randomize)
            {
                for (int i = 1; i < Path.Length; i++)
                {
                    Gizmos.DrawLine(Path[i], Path[i - 1]);
                }
                Gizmos.DrawLine(Path[0], Path[Path.Length - 1]);
            }
            else
            {
                for (int i = 0; i < Path.Length; i++)
                {
                    for (int j = i + 1; j < Path.Length; j++)
                    {
                        Gizmos.DrawLine(Path[i], Path[j]);
                    }
                }
            }
        }
    }
}

