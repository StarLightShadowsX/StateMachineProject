using System;
using UnityEngine;
#if UNITY_EDITOR  
using UnityEditor;
#endif

namespace SLS.StateMachineH
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(HiddenSerializedAttribute), true), System.Obsolete]
    public class HiddenSerializedAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //if (IsDebugInspector())
            //{
            //    // Force display of the property even if HideInInspector is applied  
            //    EditorGUI.PropertyField(position, property, label, true);
            //}
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (IsDebugInspector())
            {
                return EditorGUI.GetPropertyHeight(property, label, true);
            }
            return 0f;
        }

        private bool IsDebugInspector() => EditorPrefs.GetBool("DeveloperMode", false);
    }
#endif

}