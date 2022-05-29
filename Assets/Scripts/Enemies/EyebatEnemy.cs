using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class EyebatEnemy : MonoBehaviour 
    {
        [SerializeField] float aggroRadius;
        [SerializeField] float diveBombSpeed;
        [SerializeField] int damage;

        [SerializeField] PlayerData playerData;

        private SpriteRenderer sprite;
        private EnemyPatrol patrol;
        private IAIBehavior activeBehavior;

        private int cacheIdx = 0;
        private bool diving = false;
        private bool returningToPath = false;

        void Start()
        {
            sprite = GetComponentInChildren<SpriteRenderer>();
            activeBehavior = new AIPatrol();
            activeBehavior.Initialize(transform);
            //activeBehavior = new AIIdle(float.PositiveInfinity, null);
        }

        private void Update()
        {
            var pos = transform.position;
            activeBehavior.Update();
            sprite.flipX = pos.x < transform.position.x;

            if (diving || playerData.activePlayerObject == null)
            {
                return;
            }
            else if (returningToPath)
            {
                returningToPath = cacheIdx == (activeBehavior as AIPatrol).Index;
                return;
            }
            if (Vector3.Distance(playerData.activePlayerObject.transform.position, transform.position) <= aggroRadius)
            {
                DiveBomb();
            }
        }

        public void Damage(int dmg)
        {
            Debug.Log($"Killed {name}!");
            Destroy(gameObject);
        }

        private void ResumePatrol()
        {
            var patrol = new AIPatrol();
            patrol.Initialize(transform);
            patrol.Index = cacheIdx;
            activeBehavior = patrol;
            returningToPath = true;
            diving = false;
        }

        [ContextMenu("DiveBomb")]
        public void DiveBomb()
        {
            cacheIdx = (activeBehavior as AIPatrol).Index;
            //var db = new AIDiveBomb(target.position, 5f, ResumePatrol);
            var db = new AIDiveBomb(
                playerData.activePlayerObject.transform.position, 
                diveBombSpeed, 
                ResumePatrol
                );
            activeBehavior = db;
            activeBehavior.Initialize(transform);
            diving = true;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Player player))
            {
                player.Damage(damage);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, aggroRadius);
        }
    }

}

