using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Hackbox.UI;

namespace Hackbox.Parameters
{
    [CustomPropertyDrawer(typeof(AnyParameterListAttribute))]
    [CustomPropertyDrawer(typeof(ParameterList))]
    public class ParameterListDrawer : PropertyDrawer
    {
        private class Setup
        {
            public ParameterList List
            {
                get;
                set;
            }

            public ReorderableList Reorder
            {
                get;
                set;
            }

            public string[] ParameterNames
            {
                get;
                set;
            } = null;

            public int ParameterIndex
            {
                get;
                set;
            } = 0;
        }

        private readonly Dictionary<string, Setup> _setups = new Dictionary<string, Setup>();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            CheckInitialize(property);
            return _setups[property.propertyPath].Reorder.GetHeight();
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            CheckInitialize(property);

            position = EditorGUI.IndentedRect(position);

            SerializedObject serializedObject = property.serializedObject;

            EditorGUI.BeginChangeCheck();
            EditorGUI.BeginProperty(position, label, property);

            Setup setup = _setups[property.propertyPath];

            if (setup.ParameterNames != null)
            {
                setup.ParameterIndex = EditorGUI.Popup(new Rect(position.x, position.yMax - setup.Reorder.footerHeight, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight), setup.ParameterIndex, setup.ParameterNames);
                if (GUI.Button(new Rect(position.x + EditorGUIUtility.labelWidth, position.yMax - setup.Reorder.footerHeight, 20, EditorGUIUtility.singleLineHeight), "+"))
                {
                    Parameter parameter = CreateParameter(property, setup.ParameterNames[setup.ParameterIndex]);
                    if (parameter == null)
                    {
                        parameter = DefaultParameters.CreateDefaultAnyParameter(setup.ParameterNames[setup.ParameterIndex]);
                    }
                    setup.List.Parameters.Add(parameter);
                }
            }

            setup.Reorder.DoList(position);

            EditorGUI.EndProperty();

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(serializedObject.targetObject);
            }
        }

        protected virtual IEnumerable<string> GetParameterNames(SerializedProperty property)
        {
            GetParameterChain(property, out object parent, out Parameter[] parameterChain);
            Dictionary<string, Parameter> normalParameters = DefaultParameters.GetDefaultParameters(parent, parameterChain);
            if (normalParameters != null)
            {
                return normalParameters.Keys;
            }
            Dictionary<string, Parameter> styleParameters = DefaultParameters.GetDefaultStyleParameters(parent, parameterChain);
            if (styleParameters != null)
            {
                return styleParameters.Keys;
            }

            return null;
        }

        protected virtual Parameter CreateParameter(SerializedProperty property, string name)
        {
            GetParameterChain(property, out object parent, out Parameter[] parameterChain);
            Parameter normalParameter = DefaultParameters.CreateDefaultParameter(name, parent, parameterChain);
            if (normalParameter != null)
            {
                return normalParameter;
            }

            Parameter styleParameter = DefaultParameters.CreateDefaultStyleParameter(name, parent, parameterChain);
            if (styleParameter != null)
            {
                return styleParameter;
            }

            return null;
        }

        protected void GetParameterChain(SerializedProperty property, out object parent, out Parameter[] parameterChain)
        {
            List<Parameter> parameters = new List<Parameter>();
            object[] parents = PropertyDiscovery.GetAllParents(property);
            parent = parents.First(x => x is Preset || x is UIComponent || x is ParameterListParameter);

            foreach (object propertyParent in parents)
            {
                if (propertyParent is Parameter parentParameter)
                {
                    parameters.Add(parentParameter);
                }
            }

            parameterChain = parameters.ToArray();
        }

        private void CheckInitialize(SerializedProperty property)
        {
            if (!_setups.ContainsKey(property.propertyPath))
            {
                Initialize(property);
            }

            _setups[property.propertyPath].ParameterNames = GetParameterNames(property)?.ToArray();
            if (_setups[property.propertyPath].ParameterNames == null)
            {
                _setups[property.propertyPath].ParameterNames = DefaultParameters.AllParameterLookup.Keys.ToArray();
            }
        }

        private void Initialize(SerializedProperty property)
        {
            ParameterList obj = (ParameterList)PropertyDiscovery.GetValue(property);
            if (obj.Parameters == null)
            {
                obj.Parameters = new List<Parameter>();
            }

            ReorderableList reorder = new ReorderableList(obj.Parameters, typeof(Parameter), true, true, false, true);
            reorder.headerHeight = EditorGUIUtility.singleLineHeight;

            _setups[property.propertyPath] = new Setup() { List = obj, Reorder = reorder, ParameterIndex = 0 };

            reorder.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, property.displayName, EditorStyles.boldLabel);
            };

            reorder.footerHeight = EditorGUIUtility.singleLineHeight * 2;

            SerializedProperty parametersProperty = property.FindPropertyRelative("Parameters");
            List<float> heights = new List<float>(Enumerable.Range(0, parametersProperty.arraySize).Select(_ => 0.0f));

            reorder.elementHeightCallback = (int index) =>
            {
                while (heights.Count <= index)
                {
                    heights.Add(0.0f);
                }

                if (index < 0 || index >= parametersProperty.arraySize)
                {
                    heights[index] = EditorGUIUtility.singleLineHeight;
                }
                else
                {
                    SerializedProperty parameterProperty = parametersProperty.GetArrayElementAtIndex(index);
                    if (parameterProperty != null)
                    {
                        heights[index] = EditorGUI.GetPropertyHeight(parameterProperty);
                    }
                    else
                    {
                        heights[index] = EditorGUIUtility.singleLineHeight;
                    }
                }

                return heights[index];
            };

            reorder.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                if (index < 0 || index >= parametersProperty.arraySize)
                {
                    return;
                }

                SerializedProperty parameterProperty = parametersProperty.GetArrayElementAtIndex(index);
                SerializedProperty nameProperty = parameterProperty.FindPropertyRelative("Name");
                string parameterName = nameProperty != null ? nameProperty.stringValue : "";
                               
                Color color = GUI.color;
                float normalWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = Mathf.Min(normalWidth, 100);

                if (!_setups[property.propertyPath].ParameterNames.Contains(parameterName))
                {
                    GUI.color = Color.yellow;
                    GUIContent icon = EditorGUIUtility.IconContent("Warning", "This parameter is not expected in this section.");
                    icon.tooltip = "This parameter is not expected in this section.";
                    EditorGUI.LabelField(rect, icon);
                    rect.x += 20;
                    rect.width -= 20;
                }

                EditorGUI.PropertyField(rect, parameterProperty, true);
                EditorGUIUtility.labelWidth = normalWidth;
                GUI.color = color;
            };

            reorder.drawElementBackgroundCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                if (index < 0 || index >= parametersProperty.arraySize)
                {
                    return;
                }

                rect.height = heights[index];
                Texture2D tex = new Texture2D(1, 1);
                tex.SetPixel(0, 0, isFocused ? GUI.skin.settings.selectionColor : new Color(0, 0, 0, 0.2f));
                tex.Apply();
                if (isFocused || (index % 2) == 1)
                {
                    GUI.DrawTexture(rect, tex as Texture);
                }
            };
        }
    }

    [CustomPropertyDrawer(typeof(StyleParameterListAttribute))]
    public class StyleParameterListDrawer : ParameterListDrawer
    {
        protected override IEnumerable<string> GetParameterNames(SerializedProperty property)
        {
            GetParameterChain(property, out object parent, out Parameter[] parameterChain);
            return DefaultParameters.GetDefaultStyleParameters(parent, parameterChain)?.Keys;
        }

        protected override Parameter CreateParameter(SerializedProperty property, string name)
        {
            GetParameterChain(property, out object parent, out Parameter[] parameterChain);
            return DefaultParameters.CreateDefaultStyleParameter(name, parent, parameterChain);
        }
    }

    [CustomPropertyDrawer(typeof(NormalParameterListAttribute))]
    public class NormalParameterListDrawer : ParameterListDrawer
    {
        protected override IEnumerable<string> GetParameterNames(SerializedProperty property)
        {
            GetParameterChain(property, out object parent, out Parameter[] parameterChain);
            return DefaultParameters.GetDefaultParameters(parent, parameterChain)?.Keys;
        }

        protected override Parameter CreateParameter(SerializedProperty property, string name)
        {
            GetParameterChain(property, out object parent, out Parameter[] parameterChain);
            return DefaultParameters.CreateDefaultParameter(name, parent, parameterChain);
        }
    }

    [CustomPropertyDrawer(typeof(HeaderParameterListAttribute))]
    public class HeaderParameterListDrawer : ParameterListDrawer
    {
        protected override IEnumerable<string> GetParameterNames(SerializedProperty property)
        {
            return DefaultParameters.HeaderParameterLookup.Keys;
        }

        protected override Parameter CreateParameter(SerializedProperty property, string name)
        {
            return DefaultParameters.CreateDefaultHeaderParameter(name);
        }
    }

    [CustomPropertyDrawer(typeof(MainParameterListAttribute))]
    public class MainParameterListDrawer : ParameterListDrawer
    {
        protected override IEnumerable<string> GetParameterNames(SerializedProperty property)
        {
            return DefaultParameters.MainParameterLookup.Keys;
        }

        protected override Parameter CreateParameter(SerializedProperty property, string name)
        {
            return DefaultParameters.CreateDefaultMainParameter(name);
        }
    }
}
