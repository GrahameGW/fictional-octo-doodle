using UnityEngine;


namespace FictionalOctoDoodle.Core
{
    public class ImmortalObject : MonoBehaviour
    {
        [SerializeField] string tagToKill;
        
        private void Awake()
        {
            if (tagToKill != null)
            {
                var objs = GameObject.FindGameObjectsWithTag(tagToKill);
                for (int i = objs.Length - 1; i >= 0; i--)
                {
                    if (objs[i] == gameObject) continue;
                    Destroy(objs[i]);
                }
            }
            DontDestroyOnLoad(this.gameObject);
        }
    }
}


