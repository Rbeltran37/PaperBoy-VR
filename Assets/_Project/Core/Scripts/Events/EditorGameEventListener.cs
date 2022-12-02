// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using Core.Debug;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace Core.Events
{
    public class EditorGameEventListener : GameEventListener
    {
        [Space(10)]
        [SerializeField] private UnityEvent unityEvent;     //Doesn't actually call events

        [HideInInspector] [SerializeField] private List<Object> objects = new List<Object>();
        [HideInInspector] [SerializeField] private List<string> methodNames = new List<string>();
        [HideInInspector] [SerializeField] private List<Type> types = new List<Type>();
        [HideInInspector] [SerializeField] private List<MethodInfo> methodInfos = new List<MethodInfo>();


        private void OnValidate()
        {
            TransferCallbacksFromUnityEvent();
        }

        private void TransferCallbacksFromUnityEvent()
        {
            if (unityEvent == null)
            {
                return;
            }
            
            objects.Clear();
            types.Clear();
            methodNames.Clear();
            methodInfos.Clear();
            
            Callbacks.Clear();
            
            //Get Objects and methods
            int count = unityEvent.GetPersistentEventCount();
            for (int i = 0; i < count; i++)
            {
                objects.Add(unityEvent.GetPersistentTarget(i));
                types.Add(objects[i].GetType());
                methodNames.Add(unityEvent.GetPersistentMethodName(i));
                methodInfos.Add(types[i].GetMethod(methodNames[i]));
            }

            //Store methods in Callbacks
            for (int i = 0; i < count; i++)
            {
                MethodInfo methodInfo = methodInfos[i];
                if (methodInfo == null)
                {
                    CustomLogger.EditorOnlyWarning(nameof(TransferCallbacksFromUnityEvent), $"{nameof(methodInfo)} is null for {objects[i]}. Cannot store callback.", objects[i]);
                    continue;
                }
                Action currentCallback = (Action) Delegate.CreateDelegate(typeof(Action), objects[i], methodInfos[i]);
                Callbacks.Add(currentCallback);
            }
        }

        public override void OnEventRaised()
        {
            for (int i = Callbacks.Count - 1; i >= 0; i--)
            {
                Action currentCallback = Callbacks[i];
                if (currentCallback == null)
                {
                    continue;
                }

                try
                {
                    currentCallback.Invoke();
                }
                catch (Exception exception)
                {
                    CustomLogger.EditorOnlyError(nameof(OnEventRaised), $"{exception} | {nameof(currentCallback)}={currentCallback} in {nameof(Callbacks)}. Removing...");
                    Callbacks.Remove(currentCallback);
                    throw;
                }
            }
        }
    }
}