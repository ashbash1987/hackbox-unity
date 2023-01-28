using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json.Linq;

namespace Hackbox
{
    public class JSONViewer : EditorWindow
    {
        private Dictionary<string, bool> _foldouts = new Dictionary<string, bool>();
        private Vector2 _scroll = Vector2.zero;
        private JToken _jsonRoot = null;

        public static void ViewJSON(string json)
        {
            ViewJSON(JObject.Parse(json));
        }

        public static void ViewJSON(JToken jsonRoot)
        {
            JSONViewer window = EditorWindow.GetWindow<JSONViewer>("JSON Viewer");
            window._foldouts.Clear();
            window._jsonRoot = jsonRoot;
            window.ShowUtility();
        }

        private void OnGUI()
        {
            _scroll = EditorGUILayout.BeginScrollView(_scroll, true, true);
            DrawToken("", null, _jsonRoot);
            EditorGUILayout.EndScrollView();
        }

        private void DrawToken(string fullName, string name, JToken jToken)
        {
            fullName = $"{fullName}.{name}";
            if (jToken is JObject jObject)
            {
                DrawObject(fullName, name, jObject);
            }
            else if (jToken is JArray jArray)
            {
                DrawArray(fullName, name, jArray);
            }
            else if (jToken is JValue jValue)
            {
                DrawValue(fullName, name, jValue);
            }
        }

        private void DrawObject(string fullName, string name, JObject jObject)
        {
            if (!_foldouts.TryGetValue(fullName, out bool foldout))
            {
                _foldouts[fullName] = false;
            }
            if (!string.IsNullOrEmpty(name))
            {
                foldout = EditorGUILayout.Foldout(foldout, name);
                _foldouts[fullName] = foldout;
            }
            else
            {
                foldout = true;
            }

            if (foldout)
            {
                EditorGUI.indentLevel++;
                foreach (JProperty property in jObject.Properties())
                {
                    DrawToken(fullName, $"{property.Name}", property.Value);
                }
                EditorGUI.indentLevel--;
            }
        }

        private void DrawArray(string fullName, string name, JArray jArray)
        {
            if (!_foldouts.TryGetValue(fullName, out bool foldout))
            {
                _foldouts[fullName] = false;
            }
            if (!string.IsNullOrEmpty(name))
            {
                foldout = EditorGUILayout.Foldout(foldout, name);
                _foldouts[fullName] = foldout;
            }
            else
            {
                foldout = true;
            }

            if (foldout)
            {
                EditorGUI.indentLevel++;
                for (int arrayIndex = 0; arrayIndex < jArray.Count; ++arrayIndex)
                {
                    DrawToken(fullName, $"[{arrayIndex}]", jArray[arrayIndex]);
                }
                EditorGUI.indentLevel--;
            }
        }

        private void DrawValue(string fullName, string name, JValue jValue)
        {
            switch (jValue.Type)
            {
                case JTokenType.String:
                    EditorGUILayout.LabelField(name, $"\"{jValue}\"");
                    break;

                default:
                    EditorGUILayout.LabelField(name, jValue.ToString());
                    break;
            }
        }
    }
}
