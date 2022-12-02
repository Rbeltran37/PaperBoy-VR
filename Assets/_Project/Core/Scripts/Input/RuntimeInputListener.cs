using System;
using Core.Debug;
using UnityEngine.InputSystem;

namespace Core.Input
{
    public class RuntimeInputListener : InputListener
    {
        public void Subscribe(Action<InputAction.CallbackContext> callbackContext)
        {
            if (Callbacks.Contains(callbackContext))
            {
                CustomLogger.EditorOnlyWarning(nameof(Subscribe), $"{nameof(Callbacks)} already contains {callbackContext}. Cannot subscribe.");
                return;
            }
            
            Callbacks.Add(callbackContext);
        }
        
        public void Unsubscribe(Action<InputAction.CallbackContext> callbackContext)
        {
            if (!Callbacks.Contains(callbackContext))
            {
                CustomLogger.EditorOnlyWarning(nameof(Unsubscribe), $"{nameof(Callbacks)} doesn't contain {callbackContext}. Cannot unsubscribe.");
                return;
            }
            
            Callbacks.Remove(callbackContext);
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
                    CustomLogger.EditorOnlyError(nameof(OnEventRaised), $"{exception} | {nameof(currentCallback)}={currentCallback} in {nameof(Callbacks)}. Unsubscribing...");
                    Unsubscribe(currentCallback);
                    throw;
                }
            }
        }
    }
}
