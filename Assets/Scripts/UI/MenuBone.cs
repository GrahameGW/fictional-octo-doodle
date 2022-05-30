using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class MenuBone : MonoBehaviour
    {
        [SerializeField] float timeToLive;
        [SerializeField] float spinSpeed;

        private Rigidbody2D rb;
        private float elapsed = 0f;


        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = Random.Range(0.01f, 0.25f);
            rb.AddTorque(Random.Range(-spinSpeed, spinSpeed), ForceMode2D.Impulse);
        }

        private void Update()
        {
            elapsed += Time.deltaTime;

            if (elapsed >= timeToLive)
            {
                Destroy(gameObject);
            }
        }
    }
}

