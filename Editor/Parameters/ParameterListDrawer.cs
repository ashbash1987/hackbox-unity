using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Hackbox.Parameters
{
    [CustomPropertyDrawer(typeof(ParameterList))]
    public class ParameterListDrawer : PropertyDrawer
    {
        private readonly Dictionary<string, (ParameterList obj, ReorderableList reorder)> _setups = new Dictionary<string, (ParameterList, ReorderableList)>();

        private int _parameterIndex = 0;
        private static string[] _commonParameterNames = null;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            CheckInitialize(property);
            return _setups[property.propertyPath].reorder.GetHeight();
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            CheckInitialize(property);

            position = EditorGUI.IndentedRect(position);

            SerializedObject serializedObject = property.serializedObject;

            EditorGUI.BeginChangeCheck();
            EditorGUI.BeginProperty(position, label, property);

            (ParameterList obj, ReorderableList reorder) = _setups[property.propertyPath];

            _parameterIndex = EditorGUI.Popup(new Rect(position.x, position.yMax - reorder.footerHeight, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight), _parameterIndex, _commonParameterNames);
            if (GUI.Button(new Rect(position.x + EditorGUIUtility.labelWidth, position.yMax - reorder.footerHeight, 20, EditorGUIUtility.singleLineHeight), "+"))
            {
                Parameter parameter = CommonParameters.CreateParameter(_commonParameterNames[_parameterIndex]);
                obj.Parameters.Add(parameter);
            }

            reorder.DoList(position);

            EditorGUI.EndProperty();

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(serializedObject.targetObject);
            }
        }

        private void CheckInitialize(SerializedProperty property)
        {
            if (!_setups.ContainsKey(property.propertyPath))
            {
                Initialize(property);
            }

            if (_commonParameterNames == null)
            {
                _commonParameterNames = CommonParameters.ParameterLookup.Keys.OrderBy(x => x).ToArray();
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

            _setups[property.propertyPath] = (obj, reorder);

            reorder.headerHeight = EditorGUIUtility.singleLineHeight;
            reorder.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, property.displayName, EditorStyles.boldLabel);
            };

            reorder.footerHeight = EditorGUIUtility.singleLineHeight * 2;

            SerializedProperty parametersProperty = property.FindPropertyRelative("Parameters");            

            reorder.elementHeightCallback = (int index) =>
            {
                if (index < 0 || index >= parametersProperty.arraySize)
                {
                    return EditorGUIUtility.singleLineHeight;
                }

                SerializedProperty parameterProperty = parametersProperty.GetArrayElementAtIndex(index);
                if (parameterProperty != null)
                {
                    return EditorGUI.GetPropertyHeight(parameterProperty);
                }
                return EditorGUIUtility.singleLineHeight;
            };

            reorder.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                if (index < 0 || index >= parametersProperty.arraySize)
                {
                    return;
                }

                SerializedProperty parameterProperty = parametersProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(rect, parameterProperty, true);                
            };
        }
    }
}