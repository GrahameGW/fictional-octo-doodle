using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    [CreateAssetMenu(menuName = "Data/Move Stats")]
    public class PlayerMoveStats : ScriptableObject
    {
        public float moveSpeed;
        public float climbSpeed;
        public float jumpForce;
        public float swimSpeed;
    }
}

