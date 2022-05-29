using System.Collections;
using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class SlimeEnemy : MonoBehaviour, IDamageable
    {
        [SerializeField] int damage;
        [SerializeField] float idleTimeOnHit;
        [SerializeField] Rigidbody2D goopBulletPrefab;
        [Min(0f)]
        [SerializeField] float goopMinForce, goopMaxForce;
        [Min(0f)]
        [SerializeField] float minFireTime, maxFireTime;
        [Range(0f, 180f)]
        [SerializeField] float minFireAngle, maxFireAngle;

        [SerializeField] Transform modelRoot;

        private IAIBehavior activeBehavior;
        private Animator animator;


        void Start()
        {
            animator = GetComponent<Animator>();
            StartCoroutine(FireGoopRoutine());
            ResumePatrol();
        }

        void Update()
        {
            var pos = transform.position;
            activeBehavior.Update();
            FlipModels(pos.x > transform.position.x ? 0f : 180f);
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

        private IEnumerator FireGoopRoutine()
        {
            var elapsed = 0f;
            var timeToNextShot = 0f;
            
            while (true)
            {
                yield return null;

                if (activeBehavior is AIIdle) continue;

                if (elapsed >= timeToNextShot)
                {
                    animator.SetTrigger("attack");
                    yield return new WaitForSeconds(0.03f); // delay to map anim
                    FireGoop();
                    elapsed = 0f;
                    timeToNextShot = Random.Range(minFireTime, maxFireTime);
                }
                else
                {
                    elapsed += Time.deltaTime;
                }
            }
        }

        private void FireGoop()
        {
            var goop = Instantiate(goopBulletPrefab);
            goop.transform.position = transform.position;
            var angle = Random.Range(minFireAngle, maxFireAngle);
            var force = Random.Range(goopMinForce, goopMaxForce);

            var vec = Quaternion.Euler(0f, 0f, angle) * Vector2.right * force;
            goop.AddForce(vec, ForceMode2D.Impulse);
        }

        private void ResumePatrol()
        {
            activeBehavior = new AIPatrol();
            activeBehavior.Initialize(transform);
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
                activeBehavior = new AIIdle(idleTimeOnHit, ResumePatrol);
            }
        }
    }
}

