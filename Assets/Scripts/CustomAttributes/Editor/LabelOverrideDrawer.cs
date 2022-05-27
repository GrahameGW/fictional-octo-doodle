using UnityEditor;
using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    [CustomPropertyDrawer(typeof(LabelOverrideAttribute))]
    public class LabelOverrideDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            try
            {
                var propertyAttribute = this.attribute as LabelOverrideAttribute;
                if (SerializeArrayProperty(property) == false)
                {
                    label.text = propertyAttribute.Label;

                }
                else
                {
                    Debug.LogWarningFormat(
                        "{0}(\"{1}\") doesn't support arrays ",
                        typeof(LabelOverrideAttribute).Name,
                        propertyAttribute.Label
                    );
                }
                EditorGUI.PropertyField(position, property, label);
            }
            catch (System.Exception ex) { Debug.LogException(ex); }
        }

        private bool SerializeArrayProperty(SerializedProperty property)
        {
            string path = property.propertyPath;
            int idot = path.IndexOf('.');
            if (idot == -1) return false;
            string propName = path.Substring(0, idot);
            SerializedProperty p = property.serializedObject.FindProperty(propName);
            return p.isArray;
            //CREDITS: https://answers.unity.com/questions/603882/serializedproperty-isnt-being-detected-as-an-array.html
        }
    }
}

