// ----------------------------------------------------------------------------
// Inspired by:
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Core.EditorTools;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Events
{
    [CustomEditor(typeof(GameEvent))]
    [CanEditMultipleObjects]
    public class GameEventEditor : Editor
    {
        private int _count;
        private string _countString;
        private string _currentNamespace;
        private string _currentSubstring;
        private Type _currentType;
        private StringBuilder _stringBuilder = new StringBuilder();
        private GUILayoutOption[] _LabelWidth = new GUILayoutOption[] { GUILayout.Width(40) };
        private List<GameEventListener> _gameEventListeners;
        
        private readonly Color _color = new Color(255,119,0);

        private const float OFFSET_HEIGHT = 1.5f;
        private const float DISC_RADIUS = 1f;
        private const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.NonPublic;


        void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneGUI;
            
            Type type = typeof(GameEvent);
            FieldInfo fieldInfo = type.GetField(nameof(_gameEventListeners), BINDING_FLAGS);
            _gameEventListeners = fieldInfo.GetValue(target) as List<GameEventListener>;
        }
 
        void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            EditorHelper.CreateButton(targets, nameof(GameEvent.Raise));
            
            GUILayout.Space(40 );
            using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
            {
                _count = _gameEventListeners.Count;
                _countString = _count.ToString();
                GUILayout.Label($"List<GameEventListeners> {nameof(_gameEventListeners)}.Count:");
                GUILayout.Label(_countString, _LabelWidth);
                GUILayout.FlexibleSpace();
            }

            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                GUILayout.Space(10);
                for (int i = 0; i < _count; i++)
                {
                    bool allowSceneObjects = !EditorUtility.IsPersistent (target);
                    _stringBuilder.Append("[");
                    _stringBuilder.Append(i);
                    _stringBuilder.Append("]");

                    using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
                    {
                        Object gameEventListener = _gameEventListeners[i];
                        
                        GUILayout.Label(_stringBuilder.ToString(), _LabelWidth);
                        EditorGUILayout.ObjectField (gameEventListener, typeof(Object), allowSceneObjects);

                        _currentType = gameEventListener.GetType();
                        _currentNamespace = _currentType.Namespace;

                        _currentSubstring = _currentNamespace != null ? 
                            _currentType.ToString().Substring(_currentNamespace.Length + 1) : 
                            _currentType.ToString();
                        
                        EditorGUILayout.LabelField(_currentSubstring);
                    }
                    
                    _stringBuilder.Clear();
                }
            }
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            GameEvent gameEvent = target as GameEvent;

            Handles.color = _color;

            EditorHelper.DrawArcsAndDiscs(gameEvent, _gameEventListeners, _color, OFFSET_HEIGHT, DISC_RADIUS);
        }
    }
}