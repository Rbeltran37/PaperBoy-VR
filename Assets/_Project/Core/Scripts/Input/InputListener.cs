using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Input
{
    public abstract class InputListener : MonoBehaviour
    {
        [Tooltip("Event to register with.")]
        [SerializeField] public InputEvent inputEvent;

        public List<Action<InputAction.CallbackContext>> Callbacks = new List<Action<InputAction.CallbackContext>>();


        protected virtual void OnEnable()
        {
            inputEvent.RegisterListener(this);
        }

        protected virtual void OnDisable()
        {
            inputEvent.UnregisterListener(this);
        }

        public abstract void OnEventRaised(InputAction.CallbackContext callbackContext);
    }
}
