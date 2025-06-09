using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace SLS.StateMachineH.V5
{
    [CustomPropertyDrawer(typeof(SignalSet))]
    public class SignalSetEditor : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty list = property.FindPropertyRelative("serializedList");
            if (list == null)
                return EditorGUIUtility.singleLineHeight;

            float height = EditorGUIUtility.singleLineHeight;
            for (int i = 0; i < list.arraySize; i++)
            {
                SerializedProperty indexProperty = list.GetArrayElementAtIndex(i);
                SerializedProperty valueProperty = indexProperty.FindPropertyRelative("value");

                float valueHeight = valueProperty != null ? EditorGUI.GetPropertyHeight(valueProperty) : EditorGUIUtility.singleLineHeight;

                height += EditorGUIUtility.singleLineHeight;
                height += valueHeight;
            }
            height += EditorGUIUtility.singleLineHeight;
            return height;
        }

        SerializedProperty property;
        Rect rect;


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();

            this.property = property;
            rect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

            DrawHeader();

            SerializedProperty list = property.FindPropertyRelative("serializedList");

            for (int i = 0; i < list.arraySize; i++)
                if (!DrawItem(list.GetArrayElementAtIndex(i), i)) break;

            if (EditorGUI.EndChangeCheck())
            {
                GetReal(property).OnAfterDeserialize();
                EditorUtility.SetDirty(property.serializedObject.targetObject as UnityEngine.Object);
            }
            EditorGUI.EndProperty();
            property = null;
            rect = default;
        }

        public void DrawHeader()
        {

            GUIContent propertyLabel = new GUIContent(property.displayName);
            Rect labelRect = new Rect(rect.x, rect.y, GUI.skin.label.CalcSize(propertyLabel).x, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(labelRect, propertyLabel);

            // Calculate the width of the label text and position the button accordingly  
            Rect addButtonRect = new Rect(labelRect.xMax + 10, rect.y, 20, EditorGUIUtility.singleLineHeight);
            if (GUI.Button(addButtonRect, "+")) AddItem(property);

            rect.y += EditorGUIUtility.singleLineHeight;
        }

        public bool DrawItem(SerializedProperty item, int id)
        {
            SerializedProperty key = item.FindPropertyRelative("key");
            SerializedProperty value = item.FindPropertyRelative("value");

            if (key == null || value == null) return false;

            Rect keyRect = new Rect(rect.x+10, rect.y, rect.width - 30, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(keyRect, key, GUIContent.none);

            EditorGUI.LabelField(new Rect(rect.x, rect.y, 10, EditorGUIUtility.singleLineHeight), "*");
            Rect buttonRect = new Rect(keyRect.xMax, rect.y, 20, EditorGUIUtility.singleLineHeight);
            if (GUI.Button(buttonRect, "-"))
            {
                RemoveItem(property, id);
                return false;
            }

            rect.y += EditorGUIUtility.singleLineHeight;

            EditorGUI.PropertyField(rect, value, GUIContent.none);
            float valueHeight = value != null ? EditorGUI.GetPropertyHeight(value) : EditorGUIUtility.singleLineHeight;
            rect.y += valueHeight;

            return true;
        }


        private void AddItem(SerializedProperty property)
        {
            GetReal(property).AddNew();
            property.serializedObject.ApplyModifiedProperties();
        }

        private void RemoveItem(SerializedProperty property, int index)
        {
            SerializedProperty list = property.FindPropertyRelative("serializedList");
            if (list != null && index >= 0 && index < list.arraySize)
            {
                list.DeleteArrayElementAtIndex(index);
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        private SignalSet GetReal(SerializedProperty property) => fieldInfo.GetValue(property.serializedObject.targetObject) as SignalSet;


    }
}