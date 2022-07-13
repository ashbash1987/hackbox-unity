using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Hackbox.Parameters
{
    [CustomPropertyDrawer(typeof(ParameterList))]
    public class ParameterListDrawer : PropertyDrawer
    {
        private ParameterList _obj = null;
        private ReorderableList _reorder = null;
        private int _parameterIndex = 0;
        private static string[] _commonParameterNames = null;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            CheckInitialize(property);
            return _reorder.GetHeight();
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            CheckInitialize(property);

            position = EditorGUI.IndentedRect(position);

            SerializedObject serializedObject = property.serializedObject;

            EditorGUI.BeginChangeCheck();
            serializedObject.Update();

            EditorGUI.BeginProperty(position, label, property);

            _parameterIndex = EditorGUI.Popup(new Rect(position.x, position.yMax - _reorder.footerHeight, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight), _parameterIndex, _commonParameterNames);
            if (GUI.Button(new Rect(position.x + EditorGUIUtility.labelWidth, position.yMax - _reorder.footerHeight, 20, EditorGUIUtility.singleLineHeight), "+"))
            {
                Parameter parameter = CommonParameters.CreateParameter(_commonParameterNames[_parameterIndex]);
                _obj.Parameters.Add(parameter);
            }

            _reorder.DoList(position);

            EditorGUI.EndProperty();

            serializedObject.ApplyModifiedProperties();
            if (EditorGUI.EndChangeCheck())
            {                
                EditorUtility.SetDirty(serializedObject.targetObject);
            }
        }

        private void CheckInitialize(SerializedProperty property)
        {
            if (_obj == null || _reorder == null)
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
            _obj = (ParameterList)PropertyDiscovery.GetValue(property);

            _reorder = new ReorderableList(_obj.Parameters, typeof(Parameter), true, true, false, true);

            _reorder.headerHeight = EditorGUIUtility.singleLineHeight;
            _reorder.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, property.displayName, EditorStyles.boldLabel);
            };

            _reorder.footerHeight = EditorGUIUtility.singleLineHeight * 2;

            SerializedProperty parametersProperty = property.FindPropertyRelative("Parameters");            

            _reorder.elementHeightCallback = (int index) =>
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

            _reorder.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
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