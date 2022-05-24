using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    [CreateAssetMenu(menuName = "Data/Player")]
    public class PlayerData : ScriptableObject
    {
        public int HP;
        public int MaxHP;
        public Player activePlayerObject;
    }
}

