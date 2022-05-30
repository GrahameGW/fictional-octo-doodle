using System.Collections;
using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class GoopBullet : MonoBehaviour
    {
        [SerializeField] int damage;
        [SerializeField] SoundRandomizer sounds;

        private AudioSource audioSource;
        private Vector2 cachedPosition = default;


        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void FixedUpdate()
        {
            var tp = (Vector2)transform.position;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, tp - cachedPosition);
            cachedPosition = tp;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var player = collision.gameObject.GetComponentInParent<Player>();
            if (player != null)
            {
                player.Damage(damage);
            }

            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(Splat());
        }

        private IEnumerator Splat()
        {
            audioSource.PlayOneShot(sounds.GetClip());
            var rb = GetComponent<Rigidbody2D>();
            rb.simulated = false;
            gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
            GetComponent<Animator>().SetTrigger("splat");

            while (audioSource.isPlaying)
            {
                yield return null;
            }

            Destroy(gameObject);
        }
    }
}

