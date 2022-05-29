using System.Collections;
using UnityEngine;


namespace FictionalOctoDoodle.Core
{
    public class LimbCollectable : MonoBehaviour
    {
        [SerializeField] LimbData limbData;
        [SerializeField] SoundRandomizer sounds;

        private AudioSource audioSource;
        private SpriteRenderer sprite;
        private Collider2D col;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            sprite = GetComponent<SpriteRenderer>();
            col = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var player = collision.gameObject.GetComponentInParent<LimbAssembly>();
            if (player != null)
            {
                Debug.Log($"Player collected {name}!");
                if (player.TryAddLimb(limbData))
                {
                    StartCoroutine(CollectionRoutine());
                }
            }
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


