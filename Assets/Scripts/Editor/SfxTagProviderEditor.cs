using Obert.Audio.Runtime;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(SfxTagProvider))]
    public class SfxTagProviderEditor : UnityEditor.Editor
    {
        public static Object lastAsset;
        public override void OnInspectorGUI()
        {
            if (lastAsset && GUILayout.Button("<-"))
            {
                Selection.activeObject = lastAsset;
                EditorUtility.FocusProjectWindow();
            }
            base.OnInspectorGUI();
        }
    }
}