using UnityEngine;
using UnityEditor;
using GGJRuntime;

namespace GGJEditor
{
    [CustomEditor(typeof(WaveFunctionCollapseTilemapLogic))]
    public class WaveFunctionCollapseTilemapLogicEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SerializedProperty iterator = serializedObject.GetIterator();

            iterator.NextVisible(true);

            while(iterator.NextVisible(true))
            {
                EditorGUILayout.PropertyField(iterator);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}