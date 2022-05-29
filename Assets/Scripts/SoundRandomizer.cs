using UnityEngine;


namespace FictionalOctoDoodle.Core
{
    [CreateAssetMenu(menuName = "Sounds/Randomizer")]
    public class SoundRandomizer : ScriptableObject
    {
        [SerializeField] AudioClip[] clips;

        public AudioClip GetClip()
        {
            var i = Random.Range(0, clips.Length);
            return clips[i];
        }
    }
}


