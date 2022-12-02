using System;
using System.Collections.Generic;
using System.Reflection;
using Core.Debug;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

namespace Core.Input
{
    public class EditorInputListener : InputListener
    {
        [Space(10)]
        [SerializeField] public UnityEvent<InputAction.CallbackContext> unityEvent;     //Doesn't actually call events

        private readonly List<Object> _objects = new List<Object>();
        private readonly List<string> _methodNames = new List<string>();
        private readonly List<Type> _types = new List<Type>();
        private readonly List<MethodInfo> _methodInfos = new List<MethodInfo>();

        
        private void Awake()
        {
            TransferCallbacksFromUnityEvent();
        }

        private void TransferCallbacksFromUnityEvent()
        {
            //Get Objects and methods
            int count = unityEvent.GetPersistentEventCount();
            for (int i = 0; i < count; i++)
            {
                _objects.Add(unityEvent.GetPersistentTarget(i));
                _types.Add(_objects[i].GetType());
                _methodNames.Add(unityEvent.GetPersistentMethodName(i));
                _methodInfos.Add(_types[i].GetMethod(_methodNames[i]));
            }

            //Store methods in Callbacks
            for (int i = 0; i < count; i++)
            {
                MethodInfo methodInfo = _methodInfos[i];
                if (methodInfo == null)
                {
                    CustomLogger.EditorOnlyError(nameof(TransferCallbacksFromUnityEvent), $"{nameof(methodInfo)} is null for {_objects[i]}. Cannot store callback.", _objects[i]);
                    continue;
                }
                Action<InputAction.CallbackContext> currentCallback = (Action<InputAction.CallbackContext>) Delegate.CreateDelegate(typeof(Action<InputAction.CallbackContext>), _objects[i], _methodInfos[i]);
                Callbacks.Add(currentCallback);
            }
        }

        public override void OnEventRaised(InputAction.CallbackContext callbackContext)
        {
            for (int i = Callbacks.Count - 1; i >= 0; i--)
            {
                Action<InputAction.CallbackContext> currentCallback = Callbacks[i];
                if (currentCallback == null)
                {
                    continue;
                }

                try
                {
                    currentCallback.Invoke(callbackContext);
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
