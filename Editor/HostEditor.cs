using System;
using UnityEngine;
using UnityEditor;

namespace Hackbox
{
    [CustomEditor(typeof(Host))]
    public class HostEditor : Editor
    {
        private static GUIStyle _foldoutHeader = null;
        private static GUIStyle _foldoutBox = null;

        private Host _obj = null;

        private bool _eventsFoldout = true;
        private bool _settingsFoldout = true;
        private bool _roomStateFoldout = true;
        private bool _memberStateFoldout = true;

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
            EditorGUI.BeginChangeCheck();

            DrawFoldoutBox("Settings", ref _settingsFoldout, DrawSettingsGroup);
            DrawFoldoutBox("Events", ref _eventsFoldout, DrawEventGroup);
            DrawFoldoutBox("Room State", ref _roomStateFoldout, DrawRoomStateGroup);
            DrawFoldoutBox("Member State", ref _memberStateFoldout, DrawMemberStateGroup);

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(serializedObject.targetObject);
            }
        }

        private void DrawFoldoutBox(string header, ref bool foldoutState, Action onDraw)
        {
            if (_foldoutHeader == null)
            {
                _foldoutHeader = new GUIStyle(EditorStyles.foldoutHeader);
                _foldoutHeader.fontSize = 16;
                _foldoutHeader.fontStyle = FontStyle.Bold;
                _foldoutHeader.fixedHeight = 20;
            }

            if (_foldoutBox == null)
            {
                _foldoutBox = new GUIStyle(EditorStyles.helpBox);
                _foldoutBox.margin = new RectOffset(0, 0, 5, 5);
                _foldoutBox.padding = new RectOffset(15, 5, 5, 5);
                _foldoutBox.overflow = new RectOffset(0, 0, 0, 0);
            }

            EditorGUILayout.BeginVertical(_foldoutBox);
            foldoutState = EditorGUILayout.BeginFoldoutHeaderGroup(foldoutState, header, _foldoutHeader);
            if (foldoutState)
            {
                onDraw.Invoke();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.EndVertical();
        }

        private void DrawEventGroup()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(Host.OnRoomCreated)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(Host.OnRoomConnected)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(Host.OnRoomDisconnected)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(Host.OnRoomReconnecting)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(Host.OnRoomReconnectFailed)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(Host.OnMemberJoined)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(Host.OnMemberKicked)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(Host.OnMessage)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(Host.OnPingPong)));
        }

        private void DrawSettingsGroup()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(Host.URL)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(Host.HostName)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(Host.HostVersion)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(Host.ReloadHost)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(Host.Debugging)));
        }

        private void DrawRoomStateGroup()
        {
            if (_obj.Connected)
            {
                EditorGUILayout.LabelField("Server", _obj.URL);
                EditorGUILayout.LabelField("Room Code", _obj.RoomCode);
                EditorGUILayout.LabelField("Host User ID", _obj.UserID);
                GUI.enabled = Application.isPlaying;
                if (GUILayout.Button("Disconnect"))
                {
                    _obj.Disconnect();
                }
                GUI.enabled = true;
            }
            else
            {
                EditorGUILayout.LabelField("Disconnected");
                GUI.enabled = Application.isPlaying;
                if (GUILayout.Button("Connect"))
                {
                    _obj.Connect();
                }
                GUI.enabled = true;
            }
        }

        private void DrawMemberStateGroup()
        {
            if (_obj.HasMembers)
            {
                foreach (Member member in _obj.AllMembers)
                {
                    EditorGUILayout.BeginVertical(_foldoutBox);

                    EditorGUILayout.LabelField($"{member.Name} [{member.UserID}]", EditorStyles.boldLabel, GUILayout.ExpandWidth(true));

                    if (member.Twitch != null)
                    {
                        EditorGUILayout.LabelField($"Twitch: {member.Twitch}", EditorStyles.boldLabel, GUILayout.ExpandWidth(true));
                    }

                    if (GUILayout.Button("View State JSON"))
                    {
                        JSONViewer.ViewJSON(member.State.GenerateJSON(_obj.HostVersion));
                    }
                    EditorGUILayout.EndVertical();
                }
            }
            else
            {
                EditorGUILayout.LabelField("<No members>");
            }
        }
    }
}
