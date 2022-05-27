using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class LabelOverrideAttribute : PropertyAttribute
    {
        public string Label { get; private set; }
        public LabelOverrideAttribute(string label)
        {
            Label = label;
        }
    }
}

