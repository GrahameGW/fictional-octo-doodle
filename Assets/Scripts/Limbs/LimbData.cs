using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    [CreateAssetMenu(menuName = "Limb")]
    public class LimbData : ScriptableObject
    {
        [field: SerializeField]
        public GameObject Prefab { get; private set; }
        [field: SerializeField]
        public LimbSlot[] Slots { get; private set; }
        [field: SerializeField]
        public Sprite HudSprite { get; private set; }
    }
}

