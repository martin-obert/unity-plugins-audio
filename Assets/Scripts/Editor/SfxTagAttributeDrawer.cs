using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Obert.Audio.Runtime;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(SfxTagAttribute))]
    public class SfxTagAttributeDrawer : PropertyDrawer
    {
        private float _height;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return _height;
        }

        private readonly RenderContext _renderContext = new();


        private class RenderContext
        {
            private const string ResourceFolder = "Resources";

            public void RenderHeader(Vector2 maxTextSize, Rect bounds, SerializedProperty property, string tag,
                ref float offset)
            {
                EditorGUI.LabelField(new Rect(bounds.x, bounds.y + offset, bounds.width, maxTextSize.y), "SFX Tags");
                offset += maxTextSize.y;

                EditorGUI.LabelField(new Rect(bounds.x, bounds.y + offset, bounds.width, maxTextSize.y), tag);
                offset += maxTextSize.y;

                if (SfxTagProvider.Instance == null)
                {
                    if (GUI.Button(new Rect(bounds) { y = bounds.y + offset, height = maxTextSize.y },
                            "Create SFX tag provider"))
                    {
                        var obj = ScriptableObject.CreateInstance<SfxTagProvider>();
                        SfxTagProviderEditor.lastAsset = property.serializedObject.targetObject;
                        CheckDirectory();
                        CreateAssetAndFocus(obj);
                    }
                }
                else
                {
                    if (GUI.Button(new Rect(bounds) { y = bounds.y + offset, height = maxTextSize.y },
                            "Manage Tags"))
                    {
                        SfxTagProviderEditor.lastAsset = property.serializedObject.targetObject;
                        Selection.activeObject = SfxTagProvider.Instance as SfxTagProvider;
                        EditorUtility.FocusProjectWindow();
                    }
                }

                offset += maxTextSize.y;
            }

            private static void CreateAssetAndFocus(SfxTagProvider obj)
            {
                var path = $"Assets/{ResourceFolder}/{SfxTagProvider.Path}.asset";
                var generateUniqueAssetPath = AssetDatabase.GenerateUniqueAssetPath(path);
                AssetDatabase.CreateAsset(obj, generateUniqueAssetPath);
                AssetDatabase.SaveAssets();
                SfxTagProvider.Instance = obj;
                Selection.activeObject = obj;
                EditorUtility.FocusProjectWindow();
            }

            private static void CheckDirectory()
            {
                var path = Path.Combine(Application.dataPath, ResourceFolder);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }

            private const float CheckboxWidth = 30;

            public string[] FilterTags(Vector2 maxTextSize, Rect bounds, ref string currentFilter, string[] allTags,
                ref float offset)
            {
                if (!allTags.Any()) return allTags;

                var newFilter = EditorGUI.TextField(new Rect(bounds)
                {
                    y = bounds.y + offset,
                    height = maxTextSize.y
                }, currentFilter);
                offset += maxTextSize.y;

                currentFilter = newFilter;

                if (string.IsNullOrWhiteSpace(currentFilter)) return allTags;

                var v = currentFilter;
                return allTags.Where(x => Regex.IsMatch(x, v)).ToArray();
            }

            public void RenderCheckboxes(Vector2 maxTextSize, Rect bounds, string[] tags, string[] allTags,
                ref float offset,
                SerializedProperty serializedProperty)
            {
                var checkboxWidth = maxTextSize.x + CheckboxWidth;

                var cols = (int)(bounds.width / checkboxWidth);
                if (cols < 1)
                {
                    cols = 1;
                }


                for (var i = 0; i < allTags.Length; i += cols)
                {
                    var data = allTags.Skip(i).Take(cols).ToArray();
                    for (var j = 0; j < data.Length; j++)
                    {
                        var tag = data[j];
                        var isOn = tags.Contains(tag);
                        var wasChecked =
                            GUI.Toggle(
                                new Rect(bounds.x + checkboxWidth * j,
                                    bounds.y + offset,
                                    checkboxWidth, maxTextSize.y), isOn, tag, new GUIStyle(GUI.skin.toggle)
                                {
                                });


                        if (wasChecked == isOn) continue;
                        tags = wasChecked
                            ? SfxTagHelpers.AppendTag(tags, tag)
                            : SfxTagHelpers.RemoveTag(tags, tag);

                        serializedProperty.stringValue = SfxTagHelpers.GetTag(tags);
                        serializedProperty.serializedObject.ApplyModifiedProperties();
                    }

                    offset += maxTextSize.y;
                }
            }
        }

        private string _currentFilter;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var allTags = SfxTagProvider.Instance?.AvailableTags ?? Array.Empty<string>();
            var skinLabel = GUI.skin.label;
            var currentTags = SfxTagHelpers.GetTagValues(property.stringValue);
            var maxTextSize =
                skinLabel.CalcSize(
                    new GUIContent(allTags
                        .OrderByDescending(x => x.Length)
                        .FirstOrDefault())
                );
            var offset = 0f;
            _renderContext.RenderHeader(maxTextSize, position, property, SfxTagHelpers.GetTag(currentTags), ref offset);
            allTags = _renderContext.FilterTags(maxTextSize, position, ref _currentFilter, allTags, ref offset);
            _renderContext.RenderCheckboxes(maxTextSize, position, currentTags, allTags.OrderTags().ToArray(),
                ref offset, property);
            _height = offset;
        }
    }
}