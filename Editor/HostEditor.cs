using System;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Hackbox
{
    [CustomEditor(typeof(Host))]
    public class HostEditor : Editor
    {
        private Host _obj = null;

        private void OnEnable()
        {
            _obj = target as Host;
        }

        private void OnDisable()
        {
            _obj = null;
        }

        public override bool RequiresConstantRepaint()
        {
            return true;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.LabelField("Room Info", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Room Code", _obj.RoomCode);
            EditorGUILayout.Space();
            
            EditorGUILayout.LabelField("Members", EditorStyles.boldLabel);
            if (_obj.HasMembers)
            {
                foreach (Member member in _obj.AllMembers)
                {
                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField(member.Name, GUILayout.ExpandWidth(true));
                    EditorGUILayout.LabelField(member.UserID);
                    
                    EditorGUILayout.EndHorizontal();
                }
            }
            else
            {
                EditorGUILayout.LabelField("<No members>");
            }
        }
    }
}
