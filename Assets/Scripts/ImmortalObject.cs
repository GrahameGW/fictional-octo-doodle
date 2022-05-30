using UnityEngine;


namespace FictionalOctoDoodle.Core
{
    public class ImmortalObject : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}


