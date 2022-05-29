using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class AIPatrol : IAIBehavior
    {
        private Transform transform;
        private EnemyPatrol patrol;
        
        public int Index { get; set; }

        public void Initialize(Transform transform)
        {
            this.transform = transform;
            patrol = transform.GetComponent<EnemyPatrol>();
            Index = 0;
        }

        public void Update()
        {
            if (patrol.Path.Length <= 0) return;

            var dest = patrol.Path[Index];
            var dir = dest - transform.position;
            dir.Normalize();

            var speed = patrol.Speed * Time.deltaTime;
            transform.Translate(speed * dir);

            if (Vector3.Distance(transform.position, dest) <= speed + 0.05f)
            {
                if (patrol.Randomize)
                {
                    Index = Random.Range(0, patrol.Path.Length - 1);
                }
                else
                {
                    Index = Index == patrol.Path.Length - 1 ? 0 : Index + 1;
                }
            }
        }
    }
}


