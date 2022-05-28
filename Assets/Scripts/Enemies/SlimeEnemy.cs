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


        private IAIBehavior activeBehavior;
        private SpriteRenderer sprite;


        void Start()
        {
            activeBehavior = new AIPatrol();
            activeBehavior.Initialize(transform);
            sprite = GetComponentInChildren<SpriteRenderer>();
            StartCoroutine(FireGoopRoutine());
        }

        void Update()
        {
            var pos = transform.position;
            activeBehavior.Update();
            sprite.flipX = pos.x < transform.position.x;
        }

        public void Damage(int dmg)
        {
            Debug.Log($"Killed {name}!");
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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Player player))
            {
                player.Damage(damage);
                activeBehavior = new AIIdle(idleTimeOnHit);
            }
        }
    }
}

