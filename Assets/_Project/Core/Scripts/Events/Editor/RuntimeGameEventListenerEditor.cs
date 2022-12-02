using System;
using System.Collections.Generic;
using System.Text;
using Core.EditorTools;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Events
{
    [CustomEditor(typeof(RuntimeGameEventListener))]
    public class RuntimeGameEventListenerEditor : GameEventListenerEditor
    {
        private string _countString;
        private StringBuilder _stringBuilder = new StringBuilder();
        private GUILayoutOption[] _LabelWidth = new GUILayoutOption[] { GUILayout.Width(40) };
        private Type _currentType;
        private string _currentNamespace;
        private string _currentSubstring;
        
        private readonly Color _color = new Color(128, 0, 128);
        
        private const float OFFSET_HEIGHT = 1.5f;
        private const float DISC_RADIUS = 1;


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(40 );
            using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
            {
                _countString = Callbacks.Count.ToString();
                GUILayout.Label($"List<Action> {nameof(Callbacks)}.Count:");
                GUILayout.Label(_countString, _LabelWidth);
                GUILayout.FlexibleSpace();
            }

            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                GUILayout.Space(10);
                int i = 0;
                foreach (var item in Callbacks)
                {
                    bool allowSceneObjects = !EditorUtility.IsPersistent (target);
                    _stringBuilder.Append("[");
                    _stringBuilder.Append(i++);
                    _stringBuilder.Append("]");

                    using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
                    {
                        GUILayout.Label(_stringBuilder.ToString(), _LabelWidth);
                        EditorGUILayout.ObjectField (item.Target as Object, typeof(Object), allowSceneObjects);

                        _currentType = item.Target.GetType();
                        _currentNamespace = _currentType.Namespace;

                        _currentSubstring = _currentNamespace != null ? 
                            _currentType.ToString().Substring(_currentNamespace.Length + 1) : _currentType.ToString();
                        
                        _stringBuilder.Clear();
                        _stringBuilder.Append(_currentSubstring);
                        _stringBuilder.Append(".");
                        _stringBuilder.Append(item.Method.Name);
                        EditorGUILayout.LabelField(_stringBuilder.ToString());
                    }
                    
                    _stringBuilder.Clear();
                }
            }
        }

        private void OnSceneGUI()
        {
            bool isPlaying = EditorApplication.isPlaying;
            GUI.enabled = isPlaying;
            if (!isPlaying)
            {
                return;
            }
            
            if (target is RuntimeGameEventListener runtimeGameEventListener)
            {
                List<Object> actionTargets = new List<Object>();
                foreach (Action callback in Callbacks)
                {
                    if (callback.Target is Object callbackTarget)
                    {
                        actionTargets.Add(callbackTarget);
                    }
                }
                EditorHelper.DrawArcsAndDiscs(runtimeGameEventListener, actionTargets, _color, OFFSET_HEIGHT, DISC_RADIUS);
            }
        }
    }
}
