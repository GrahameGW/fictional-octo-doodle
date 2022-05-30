using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class Spikes : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Player player))
            {
                player.Damage(int.MaxValue);
            }
        }
    }
}

