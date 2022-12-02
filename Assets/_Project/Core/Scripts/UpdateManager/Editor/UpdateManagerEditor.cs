using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Core.Debug;
using Core.EditorTools;
using Core.Sets;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;

namespace Core.UpdateManager
{
    [CustomEditor(typeof(UpdateManager))]
    public class UpdateManagerEditor : Editor
    {
        private bool _showFixedUpdate = true;
        private bool _showUpdate = true;
        private bool _showLateUpdate = true;
        private bool _showSmartUpdate = true;
        private string _countString;
        private StringBuilder _stringBuilder = new StringBuilder();
        private GUILayoutOption[] _LabelWidth = new GUILayoutOption[] { GUILayout.Width(40) };
        private UpdateRuntimeSet _fixedUpdateRuntimeSet;
        private UpdateRuntimeSet _updateRuntimeSet;
        private UpdateRuntimeSet _lateUpdateRuntimeSet;
        private UpdateRuntimeSet _smartUpdateRuntimeSet;
        
        private static readonly Color _fixedUpdateColor = Color.green;
        private static readonly Color _updateColor = Color.yellow;
        private static readonly Color _lateUpdateColor = Color.red;
        private static readonly Color _smartUpdateColor = Color.cyan;
        
        private const float FIXED_UPDATE_OFFSET = 1f;
        private const float UPDATE_OFFSET = 1.5f;
        private const float LATE_UPDATE_OFFSET = 2f;
        private const float SMART_UPDATE_OFFSET = 2.5f;
        private const float FIXED_UPDATE_RADIUS = 1.25f;
        private const float UPDATE_RADIUS = 1.5f;
        private const float LATE_UPDATE_RADIUS = 1.75f;
        private const float SMART_UPDATE_RADIUS = 2f;
        private const string ITEMS_COUNT_LABEL = "List<IUpdateable> Items.Count:";
        private const string FOLDOUT_SHOW_TEXT = "Show";
        private const string FOLDOUT_HIDE_TEXT = "Hide";
        private const string FIXED_UPDATE = "FixedUpdate";
        private const string LATE_UPDATE = "LateUpdate";
        private const string UPDATE = "Update";
        private const string SMART_UPDATE = "SmartUpdate";
        private const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.NonPublic;


        private void OnEnable()
        {
            Type type = typeof(UpdateManager);
            FieldInfo fieldInfo = type.GetField(nameof(_fixedUpdateRuntimeSet).Substring(1), BINDING_FLAGS);
            _fixedUpdateRuntimeSet = fieldInfo.GetValue(target) as UpdateRuntimeSet;
            
            fieldInfo = type.GetField(nameof(_updateRuntimeSet).Substring(1), BINDING_FLAGS);
            _updateRuntimeSet = fieldInfo.GetValue(target) as UpdateRuntimeSet;
            
            fieldInfo = type.GetField(nameof(_lateUpdateRuntimeSet).Substring(1), BINDING_FLAGS);
            _lateUpdateRuntimeSet = fieldInfo.GetValue(target) as UpdateRuntimeSet;
            
            fieldInfo = type.GetField(nameof(_smartUpdateRuntimeSet).Substring(1), BINDING_FLAGS);
            _smartUpdateRuntimeSet = fieldInfo.GetValue(target) as UpdateRuntimeSet;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        
            UpdateManager updateManager = (UpdateManager)target;
            
            bool allowSceneObjects = !EditorUtility.IsPersistent(target);

            GUILayout.Space(10);
            RuntimeSetUI(_fixedUpdateRuntimeSet, allowSceneObjects, _fixedUpdateColor, ref _showFixedUpdate, FIXED_UPDATE);
            GUILayout.Space(10);
            
            RuntimeSetUI(_updateRuntimeSet, allowSceneObjects, _updateColor, ref _showUpdate, UPDATE);
            GUILayout.Space(10);
            
            RuntimeSetUI(_lateUpdateRuntimeSet, allowSceneObjects, _lateUpdateColor, ref _showLateUpdate, LATE_UPDATE);
            GUILayout.Space(10);
            
            RuntimeSetUI(_smartUpdateRuntimeSet, allowSceneObjects, _smartUpdateColor, ref _showSmartUpdate, SMART_UPDATE);
            GUILayout.Space(10);
        }

        private void RuntimeSetUI(UpdateRuntimeSet runtimeSet, bool allowSceneObjects, Color contentColor, ref bool foldoutBool, string updateName)
        {
            GUI.contentColor = contentColor;

            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                _countString = runtimeSet.Items.Count.ToString();
                EditorGUILayout.LabelField(updateName, EditorStyles.boldLabel);

                using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
                {
                    GUILayout.Label(ITEMS_COUNT_LABEL);
                    GUILayout.Label(_countString, _LabelWidth);
                    GUILayout.FlexibleSpace();
                }
                
                string contentString = !foldoutBool ? FOLDOUT_SHOW_TEXT : FOLDOUT_HIDE_TEXT;
                foldoutBool = EditorGUILayout.Foldout(foldoutBool, contentString);
                if (!foldoutBool)
                {
                    return;
                }
                
                int i = 0;
                foreach (var item in runtimeSet.Items)
                {
                    _stringBuilder.Append("[");
                    _stringBuilder.Append(i++);
                    _stringBuilder.Append("]");

                    using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
                    {
                        GUILayout.Label(_stringBuilder.ToString(), _LabelWidth);
                        EditorGUILayout.ObjectField(item as Object, typeof(Object), allowSceneObjects);
                    }

                    _stringBuilder.Clear();
                }
            }
        }

        private void OnSceneGUI()
        {
            UpdateManager updateManager = (UpdateManager) target;

            if (_showFixedUpdate)
            {
                DrawBezierToUpdateables(_fixedUpdateRuntimeSet, _fixedUpdateColor, FIXED_UPDATE_OFFSET, FIXED_UPDATE_RADIUS);
            }
            
            if (_showUpdate)
            {
                DrawBezierToUpdateables(_updateRuntimeSet, _updateColor, UPDATE_OFFSET, UPDATE_RADIUS);

            }
            
            if (_showLateUpdate)
            {
                DrawBezierToUpdateables(_lateUpdateRuntimeSet, _lateUpdateColor, LATE_UPDATE_OFFSET, LATE_UPDATE_RADIUS);
            }
            
            if (_showSmartUpdate)
            {
                DrawBezierToUpdateables(_smartUpdateRuntimeSet, _smartUpdateColor, SMART_UPDATE_OFFSET, SMART_UPDATE_RADIUS);
            }
        }

        private void DrawBezierToUpdateables(UpdateRuntimeSet currentUpdateRuntime, Color color, float offsetHeight, float discRadius)
        {
            if (!currentUpdateRuntime)
            {
                CustomLogger.EditorOnlyWarning(nameof(DrawBezierToUpdateables), $"{nameof(currentUpdateRuntime)} is null.");
                return;
            }

            UpdateManager updateManager = (UpdateManager) target;

            Handles.zTest = CompareFunction.LessEqual;
            Handles.color = color;

            List<Object> objects = new List<Object>();
            foreach (IUpdateable updateable in currentUpdateRuntime.Items)
            {
                if (updateable is Object currentObject)
                {
                    objects.Add(currentObject);
                }
            }
            
            EditorHelper.DrawArcsAndDiscs(updateManager, objects, color, offsetHeight, discRadius);
        }  
    }
}
