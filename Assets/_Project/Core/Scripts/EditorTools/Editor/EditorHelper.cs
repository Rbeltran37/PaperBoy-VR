using System;
using System.Collections.Generic;
using System.Reflection;
using Core.Debug;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;

namespace Core.EditorTools
{
    public static class EditorHelper
    {
        private static readonly Vector3 _defaultStartPosition = new Vector3(0, DEFAULT_HEIGHT, 0);
        
        private const float START_TANGENT_MULTIPLIER = .15f;
        private const float END_TANGENT_MULTIPLIER = .85f;
        private const int DEFAULT_HEIGHT = 10;


        public static void CreateButton(Object[] targets, string methodName)
        {
            if (GUILayout.Button(methodName))
            {
                foreach (Object currentObject in targets)
                {
                    Type currentType = currentObject.GetType();
                    MethodInfo currentMethodInfo = currentType.GetMethod(methodName);
                    if (currentMethodInfo == null)
                    {
                        CustomLogger.EditorOnlyError(nameof(CreateButton), $"MethodInfo for {currentType.Name}.{methodName} is null. Unable to Create Button");
                        continue;
                    }
                    
                    Action currentCallback = (Action) Delegate.CreateDelegate(typeof(Action), currentObject, currentMethodInfo);
                    currentCallback.Invoke();
                }
            }
        }
        
        public static void CreateButton(Object target, string methodName)
        {
            if (GUILayout.Button(methodName))
            {
                Type currentType = target.GetType();
                MethodInfo currentMethodInfo = currentType.GetMethod(methodName);
                if (currentMethodInfo == null)
                {
                    CustomLogger.EditorOnlyError(nameof(CreateButton), $"MethodInfo for {currentType.Name}.{methodName} is null. Unable to Create Button");
                    return;
                }
                    
                Action currentCallback = (Action) Delegate.CreateDelegate(typeof(Action), target, currentMethodInfo);
                currentCallback.Invoke();
            }
        }

        public static void DrawArcsAndDiscs(Object startObject, IEnumerable<Object> endObjects, Color color, float offsetHeight, float discRadius)
        {
            Handles.zTest = CompareFunction.LessEqual;
            Handles.color = color;

            Vector3 startPosition = _defaultStartPosition;
            if (startObject is MonoBehaviour startMonoBehaviour)
            {
                startPosition = startMonoBehaviour.transform.position;
            }

            List<Vector3> endPositions = new List<Vector3>();
            foreach (Object currentObject in endObjects)
            {
                if (currentObject is MonoBehaviour currentEndMonoBehaviour)
                {
                    endPositions.Add(currentEndMonoBehaviour.transform.position);
                }
            }
                
            for (int i = endPositions.Count - 1; i >= 0; i--)
            {
                Vector3 currentEndPosition = endPositions[i];
                Vector3 offset = Vector3.up * (Math.Max(startPosition.y, currentEndPosition.y) + offsetHeight);
                Handles.DrawBezier(
                    startPosition,
                    currentEndPosition,
                    (currentEndPosition - startPosition) * START_TANGENT_MULTIPLIER + offset,
                    (currentEndPosition - startPosition) * END_TANGENT_MULTIPLIER + offset,
                    color,
                    EditorGUIUtility.whiteTexture,
                    1f
                );
                
                Handles.DrawWireDisc(currentEndPosition, Vector3.up, discRadius, 1f);
            }
        }
        
        public static void DrawArcsAndDiscs(Object startObject, Object endObject, Color color, float offsetHeight, float discRadius)
        {
            Handles.zTest = CompareFunction.LessEqual;
            Handles.color = color;

            Vector3 startPosition = _defaultStartPosition;
            if (startObject is MonoBehaviour startMonoBehaviour)
            {
                startPosition = startMonoBehaviour.transform.position;
            }

            if (endObject is MonoBehaviour currentMonoBehaviour)
            {
                Vector3 currentEndPosition = currentMonoBehaviour.transform.position;
                Vector3 offset = Vector3.up * (Math.Max(startPosition.y, currentEndPosition.y) + offsetHeight);
                Handles.DrawBezier(
                    startPosition,
                    currentEndPosition,
                    (currentEndPosition - startPosition) * START_TANGENT_MULTIPLIER + offset,
                    (currentEndPosition - startPosition) * END_TANGENT_MULTIPLIER + offset,
                    color,
                    EditorGUIUtility.whiteTexture,
                    1f
                );
                
                Handles.DrawWireDisc(currentEndPosition, Vector3.up, discRadius, 1f);
            }
        }
    }
}
