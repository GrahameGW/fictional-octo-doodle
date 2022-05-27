using UnityEngine;


namespace FictionalOctoDoodle.Core
{
    public class LimbCollectable : MonoBehaviour
    {
        [SerializeField] LimbData limbData;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var player = collision.gameObject.GetComponentInParent<LimbAssembly>();
            if (player != null)
            {
                Debug.Log($"Player collected {name}!");
                if (player.TryAddLimb(limbData))
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}


