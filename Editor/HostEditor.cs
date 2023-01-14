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

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Room Info", EditorStyles.boldLabel);
            if (_obj.Connected)
            {
                EditorGUILayout.LabelField("Room Code", _obj.RoomCode);
                EditorGUILayout.LabelField("Host User ID", _obj.UserID);
                if (Application.isPlaying && GUILayout.Button("Disconnect"))
                {
                    _obj.Disconnect();
                }
            }
            else
            {
                EditorGUILayout.LabelField("Disconnected");
                if (Application.isPlaying && GUILayout.Button("Connect"))
                {
                    _obj.Connect();
                }
            }
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
