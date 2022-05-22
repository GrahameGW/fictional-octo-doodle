using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class Spikes : MonoBehaviour
    {
        [SerializeField] int damage;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out PlayerCombat player))
            {
                player.Damage(damage);
            }
        }
    }
}

