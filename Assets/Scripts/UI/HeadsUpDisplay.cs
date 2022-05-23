using TMPro;
using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class HeadsUpDisplay : MonoBehaviour
    {
        [SerializeField] PlayerData data;
        [SerializeField] TextMeshProUGUI hpText;


        private void Update()
        {
            hpText.text = $"HP: {data.HP}/{data.MaxHP}";
        }
    }
}

