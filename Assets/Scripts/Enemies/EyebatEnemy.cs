using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class EyebatEnemy : MonoBehaviour, IDamageable
    {
        [SerializeField] float aggroRadius;
        [SerializeField] float diveBombSpeed;
        [SerializeField] int damage;

        [SerializeField] PlayerData playerData;
        [SerializeField] Transform modelRoot;
        [SerializeField] SoundRandomizer sounds;
        [SerializeField] SoundRandomizer hitBySounds;

        private Animator animator;
        private AudioSource audioSource;
        private IAIBehavior activeBehavior;

        private int cacheIdx = 0;
        private bool diving = false;
        private bool returningToPath = false;


        void Start()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            activeBehavior = new AIPatrol();
            activeBehavior.Initialize(transform);
        }

        private void Update()
        {
            var pos = transform.position;
            activeBehavior.Update();
            FlipModels(pos.x > transform.position.x ? 0f : 180f);

            if (diving || playerData.activePlayerObject == null || activeBehavior is AIIdle)
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
            GetComponent<Rigidbody2D>().simulated = false;
            audioSource.PlayOneShot(hitBySounds.GetClip());
            animator.SetBool("blinkEnabled", false);
            animator.SetTrigger("die");
            foreach (Collider2D c in GetComponentsInChildren<Collider2D>())
            {
                c.enabled = false;
            }
            activeBehavior = new AIIdle(float.PositiveInfinity, null);
            activeBehavior.Initialize(transform);
        }

        private void ResumePatrol()
        {
            var patrol = new AIPatrol();
            patrol.Initialize(transform);
            patrol.Index = cacheIdx;
            activeBehavior = patrol;
            returningToPath = true;
            diving = false;
            animator.SetTrigger("exitDive");
        }

        public void OnDeathAnimComplete()
        {
            Destroy(gameObject);
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
            animator.SetTrigger("enterDive");
            audioSource.PlayOneShot(sounds.GetClip());
        }

        private void FlipModels(float degrees)
        {
            modelRoot.eulerAngles = new Vector3(
                transform.eulerAngles.x,
                degrees,
                transform.eulerAngles.z
                );
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

