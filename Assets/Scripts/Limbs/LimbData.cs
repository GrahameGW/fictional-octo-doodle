using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    [CreateAssetMenu(menuName = "Data/Limb")]
    public class LimbData : ScriptableObject
    {
        [field: SerializeField]
        public GameObject Prefab { get; private set; }
        [field: SerializeField]
        public LimbSlot[] Slots { get; private set; }
        [field: SerializeField]
        public Sprite HudSprite { get; private set; }

        public float moveSpeedModifier;
        public float climbSpeedModifier;
        public float swimSpeedModifier;
        public float jumpForceModifier;
    }
}

