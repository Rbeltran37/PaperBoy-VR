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

namespace Core.Input
{
    [CustomEditor(typeof(InputEvent))]
    [CanEditMultipleObjects]
    public class InputEventEditor : Editor
    {
        private string _countString;
        private string _currentNamespace;
        private string _currentSubstring;
        private Type _currentType;
        private StringBuilder _stringBuilder = new StringBuilder();
        private GUILayoutOption[] _LabelWidth = new GUILayoutOption[] { GUILayout.Width(40) };
        private List<InputListener> _inputListeners;
        
        private readonly Color _color = new Color(255,119,0);
        
        private const float OFFSET_HEIGHT = 1.5f;
        private const float DISC_RADIUS = 1f;
        private const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.NonPublic;


        private void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneGUI;
            
            Type type = typeof(InputEvent);
            FieldInfo fieldInfo = type.GetField(nameof(_inputListeners), BINDING_FLAGS);
            _inputListeners = fieldInfo.GetValue(target) as List<InputListener>;
        }
 
        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            EditorHelper.CreateButton(targets, nameof(InputEvent.Raise));
            
            InputEvent inputEvent = target as InputEvent;
            
            GUILayout.Space(40 );
            using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
            {
                _countString = _inputListeners.Count.ToString();
                GUILayout.Label($"List<InputListeners> {nameof(_inputListeners)}.Count:");
                GUILayout.Label(_countString, _LabelWidth);
                GUILayout.FlexibleSpace();
            }

            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                GUILayout.Space(10);
                int i = 0;
                foreach (InputListener inputListener in _inputListeners)
                {
                    bool allowSceneObjects = !EditorUtility.IsPersistent (target);
                    _stringBuilder.Append("[");
                    _stringBuilder.Append(i++);
                    _stringBuilder.Append("]");

                    using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
                    {
                        GUILayout.Label(_stringBuilder.ToString(), _LabelWidth);
                        EditorGUILayout.ObjectField (inputListener, typeof(Object), allowSceneObjects);

                        _currentType = inputListener.GetType();
                        _currentNamespace = _currentType.Namespace;

                        _currentSubstring = _currentNamespace != null ? 
                            _currentType.ToString().Substring(_currentNamespace.Length + 1) : _currentType.ToString();
                        
                        EditorGUILayout.LabelField(_currentSubstring);
                    }
                    
                    _stringBuilder.Clear();
                }
            }
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            InputEvent inputEvent = target as InputEvent;

            Handles.color = _color;
            
            EditorHelper.DrawArcsAndDiscs(inputEvent, _inputListeners, _color, OFFSET_HEIGHT, DISC_RADIUS);
        }
    }
}