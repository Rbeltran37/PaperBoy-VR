using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Core.EditorTools;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;

namespace Core.Events
{
    [CustomEditor(typeof(EditorGameEventListener))]
    public class EditorGameEventListenerEditor : GameEventListenerEditor
    {
        private string _countString;
        private int _count;
        private StringBuilder _stringBuilder = new StringBuilder();
        private GUILayoutOption[] _LabelWidth = new GUILayoutOption[] { GUILayout.Width(40) };
        private Type _currentType;
        private string _currentNamespace;
        private string _currentSubstring;
        
        private Color _unityEventcolor = Color.blue;
        private Color _callbackColor = Color.cyan;
        private UnityEvent _unityEvent;
        
        private const float OFFSET_HEIGHT = 1.5f;
        private const float DISC_RADIUS = 1;
        private const string CALLBACKS_NAME = "Callbacks";
        private const string UNITY_EVENT_NAME = "unityEvent";
        private const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.NonPublic;


        protected override void OnEnable()
        {
            base.OnEnable();
            
            Type type = typeof(EditorGameEventListener);
            FieldInfo fieldInfo = type.GetField(UNITY_EVENT_NAME, BINDING_FLAGS);
            _unityEvent = fieldInfo.GetValue(target) as UnityEvent;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            GUILayout.Space(40 );
            using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
            {
                _count = Callbacks.Count;
                _countString = _count.ToString();
                GUILayout.Label($"List<Action> {CALLBACKS_NAME}.Count:");
                GUILayout.Label(_countString, _LabelWidth);
                GUILayout.FlexibleSpace();
            }

            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                int i = 0;
                foreach (Action callback in Callbacks)
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
            if (_unityEvent == null)
            {
                return;
            }
            
            EditorGameEventListener editorGameEventListener = (EditorGameEventListener) target;
            
            Handles.zTest = CompareFunction.LessEqual;

            bool isInPlayMode = EditorApplication.isPlaying;
            Color currentColor = isInPlayMode ? _callbackColor : _unityEventcolor;
            Handles.color = currentColor;
            
            List<Object> objectList = new List<Object>();
            int count = isInPlayMode ? Callbacks.Count : _unityEvent.GetPersistentEventCount();
            for (int i = count - 1; i >= 0; i--)
            {
                Object currentObject = isInPlayMode ? Callbacks[i].Target as Object : _unityEvent.GetPersistentTarget(i);
                if (currentObject == null)
                {
                    continue;
                }

                objectList.Add(currentObject);
            }
            
            EditorHelper.DrawArcsAndDiscs(editorGameEventListener, objectList, currentColor, OFFSET_HEIGHT, DISC_RADIUS);
        }
    }
}
