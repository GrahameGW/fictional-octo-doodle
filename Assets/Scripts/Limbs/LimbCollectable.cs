using System.Collections;
using UnityEngine;


namespace FictionalOctoDoodle.Core
{
    public class LimbCollectable : MonoBehaviour
    {
        [field: SerializeField]
        public bool Persists { get; set; }
        
        [SerializeField] LimbData limbData;
        [SerializeField] SoundRandomizer sounds;
        [Min(0f)]
        [SerializeField] float minFireForce, maxFireForce;
        [Range(0f, 180f)]
        [SerializeField] float minFireAngle, maxFireAngle;
        [SerializeField] float hoverHeight;

        private AudioSource audioSource;
        private SpriteRenderer sprite;
        private Collider2D col;
        private bool canCollect = false;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            sprite = GetComponent<SpriteRenderer>();
            col = GetComponent<Collider2D>();

            Launch(GetComponent<Rigidbody2D>());
        }

        private void Launch(Rigidbody2D rb)
        {
            var angle = Random.Range(minFireAngle, maxFireAngle);
            var force = Random.Range(minFireForce, maxFireForce);

            var vec = Quaternion.Euler(0f, 0f, angle) * Vector2.right * force;
            rb.AddForce(vec, ForceMode2D.Impulse);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (canCollect)
            {
                var player = collision.gameObject.GetComponentInParent<LimbAssembly>();
                if (player != null)
                {
                    canCollect = false;
                    if (player.TryAddLimb(limbData))
                    {
                        StartCoroutine(CollectionRoutine());
                    }
                    else
                    {
                        StartCoroutine(ResetCanCollect());
                    }
                }
            }
            
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                if (!Persists)
                {
                    Destroy(gameObject);
                    return;
                }
                
                canCollect = true;
                Destroy(GetComponent<Rigidbody2D>());
                transform.position = transform.position + Vector3.up * hoverHeight;
            }
        }

        private IEnumerator ResetCanCollect()
        {
            yield return null;
            canCollect = true;
        }

        private IEnumerator CollectionRoutine()
        {
            sprite.enabled = false;
            col.enabled = false;
            var clip = sounds.GetClip();
            audioSource.PlayOneShot(clip);

            while (audioSource.isPlaying)
            {
                yield return null;
            }

            Destroy(gameObject);
        }
    }
}


