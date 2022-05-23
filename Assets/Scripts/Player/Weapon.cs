using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] int damage;
        [SerializeField] float duration;
        [SerializeField] LayerMask layerToHit;

        private float elapsed;


        private void OnEnable()
        {
            elapsed = 0f;
        }
        private void Update()
        {
            elapsed += Time.deltaTime;
            if (elapsed >= duration)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (layerToHit != (layerToHit | (1 << collision.gameObject.layer))) // not in layermask
            {
                return;
            }

            var victim = collision.GetComponentInParent<IDamageable>();

            if (victim != null)
            {
                victim.Damage(damage);
            }
        }
    }
}

