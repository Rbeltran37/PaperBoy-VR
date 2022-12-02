using System;
using System.Collections.Generic;
using System.Text;
using Core.EditorTools;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;

namespace Core.Input
{
    [CustomEditor(typeof(EditorInputListener))]
    [CanEditMultipleObjects]
    public class EditorInputListenerEditor : Editor
    {
        private string _countString;
        private StringBuilder _stringBuilder = new StringBuilder();
        private GUILayoutOption[] _LabelWidth = new GUILayoutOption[] { GUILayout.Width(40) };
        private List<Action<InputAction.CallbackContext>> _callbacks;
        private Type _currentType;
        private string _currentNamespace;
        private string _currentSubstring;
        private Color _unityEventcolor = Color.blue;
        private Color _callbackColor = Color.cyan;
        
        private const float OFFSET_HEIGHT = 1.5f;
        private const float DISC_RADIUS = 1;

        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            EditorInputListener editorInputListener = (EditorInputListener)target;
            
            GUILayout.Space(40 );
            using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
            {
                _callbacks = editorInputListener.Callbacks;
                _countString = _callbacks.Count.ToString();
                GUILayout.Label($"List<Action> {nameof(editorInputListener.Callbacks)}.Count:");
                GUILayout.Label(_countString, _LabelWidth);
                GUILayout.FlexibleSpace();
            }

            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                GUILayout.Space(10);
                int i = 0;
                foreach (Action<InputAction.CallbackContext> callback in editorInputListener.Callbacks)
                {
                    bool allowSceneObjects = !EditorUtility.IsPersistent (target);
                    _stringBuilder.Append("[");
                    _stringBuilder.Append(i++);
                    _stringBuilder.Append("]");

                    using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
                    {
                        GUILayout.Label(_stringBuilder.ToString(), _LabelWidth);
                        EditorGUILayout.ObjectField (callback.Target as Object, typeof(Object), allowSceneObjects);

                        _currentType = callback.Target.GetType();
                        _currentNamespace = _currentType.Namespace;

                        _currentSubstring = _currentNamespace != null ? 
                            _currentType.ToString().Substring(_currentNamespace.Length + 1) : _currentType.ToString();
                        
                        _stringBuilder.Clear();
                        _stringBuilder.Append(_currentSubstring);
                        _stringBuilder.Append(".");
                        _stringBuilder.Append(callback.Method.Name);
                        EditorGUILayout.LabelField(_stringBuilder.ToString());
                    }
                    
                    _stringBuilder.Clear();
                }
            }
        }

        private void OnSceneGUI()
        {
            EditorInputListener editorInputListener = (EditorInputListener) target;
            
            Handles.zTest = CompareFunction.LessEqual;

            bool isInPlayMode = EditorApplication.isPlaying;
            Color currentColor = isInPlayMode ? _callbackColor : _unityEventcolor;
            Handles.color = currentColor;
            
            UnityEvent<InputAction.CallbackContext> currentUnityEvent = editorInputListener.unityEvent;
            List<Object> objectList = new List<Object>();
            List<Action<InputAction.CallbackContext>> callbacks = editorInputListener.Callbacks;
            int count = isInPlayMode ? callbacks.Count : currentUnityEvent.GetPersistentEventCount();
            for (int i = count - 1; i >= 0; i--)
            {
                Object currentObject = isInPlayMode ? callbacks[i].Target as Object : currentUnityEvent.GetPersistentTarget(i);
                if (currentObject == null)
                {
                    continue;
                }

                objectList.Add(currentObject);
            }
            
            EditorHelper.DrawArcsAndDiscs(editorInputListener, objectList, currentColor, OFFSET_HEIGHT, DISC_RADIUS);
        }
    }
}
