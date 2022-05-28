using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class GoopBullet : MonoBehaviour
    {
        [SerializeField] int damage;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var player = collision.gameObject.GetComponentInParent<Player>();
            if (player != null)
            {
                player.Damage(damage);
            }

            Destroy(gameObject);
        }
    }
}

