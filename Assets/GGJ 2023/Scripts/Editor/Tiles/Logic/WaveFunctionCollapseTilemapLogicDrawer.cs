using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using GGJRuntime;

namespace GGJEditor
{
    [CustomPropertyDrawer(typeof(WaveFunctionCollapseTilemapLogic))]
    public class WaveFunctionCollapseTilemapLogicDrawer : PropertyDrawer
    {
        //private Dictionary<int, SerializedObject> serializedObjects = new Dictionary<int, SerializedObject>();
        //private static Dictionary<int, Editor> editors = new Dictionary<int, Editor>();
        private Editor cachedEditor = null;

        /*public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = base.GetPropertyHeight(property, label);

            if(property.isExpanded && property.objectReferenceValue != null)
            {
                if(!serializedObjects.TryGetValue(property.objectReferenceInstanceIDValue, out SerializedObject so))
                {
                    so = new SerializedObject(property.objectReferenceValue);
                    serializedObjects.Add(property.objectReferenceInstanceIDValue, so);
                }

                SerializedProperty iterator = so.GetIterator();

                iterator.NextVisible(true);

                while(iterator.NextVisible(true))
                {
                    height += EditorGUI.GetPropertyHeight(iterator);
                    height += EditorGUIUtility.standardVerticalSpacing;
                }
            }

            return height;
        }*/


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;

            if(property.objectReferenceValue == null)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }
            else
            {
                EditorGUI.indentLevel++;
                EditorGUI.PropertyField(position, property, label);
                EditorGUI.indentLevel--;
            }

            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, GUIContent.none, false);

            if(!property.isExpanded) return;

            Editor.CreateCachedEditor(property.objectReferenceValue, null, ref cachedEditor);

            if(cachedEditor == null) return;

            EditorGUI.indentLevel++;
            cachedEditor.OnInspectorGUI();
            EditorGUI.indentLevel--;
        }


        /*private void DrawLogicFields(Rect position, SerializedProperty property)
        {

        }*/
    }
}