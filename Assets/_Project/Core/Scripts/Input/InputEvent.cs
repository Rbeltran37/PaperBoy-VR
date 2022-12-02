using System;
using System.Collections.Generic;
using Core.Debug;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Input
{
    [CreateAssetMenu(fileName = "InputEvent_", menuName = "ScriptableObjects/Core/Input/InputEvent", order = 1)]
    public class InputEvent : ScriptableObject
    {
        // The list of listeners that this event will notify if it is raised.
        private readonly List<InputListener> _inputListeners = new List<InputListener>();
        
        
        public void Raise(InputAction.CallbackContext callbackContext)
        {
            for (int i = _inputListeners.Count - 1; i >= 0; i--)
            {
                InputListener currentInputListener = _inputListeners[i];
                if (currentInputListener == null)
                {
                    continue;
                }
                
                try
                {
                    currentInputListener.OnEventRaised(callbackContext);
                }
                catch (Exception exception)
                {
                    CustomLogger.EditorOnlyError(nameof(Raise), $"{exception} | {nameof(currentInputListener)}={currentInputListener} in {_inputListeners}. Unregistering...");
                    UnregisterListener(currentInputListener);
                    throw;
                }
            }
        }

        public void RegisterListener(InputListener listener)
        {
            if (_inputListeners.Contains(listener))
            {
                CustomLogger.EditorOnlyWarning(nameof(RegisterListener), $"{nameof(_inputListeners)} already contains {listener}. Cannot register.");
                return;
            }
            
            _inputListeners.Add(listener);
        }

        public void UnregisterListener(InputListener listener)
        {
            if (!_inputListeners.Contains(listener))
            {
                CustomLogger.EditorOnlyWarning(nameof(UnregisterListener), $"{nameof(_inputListeners)} doesn't contains {listener}. Cannot unregister.");
                return;
            }
            
            _inputListeners.Remove(listener);
        }
    }
}
