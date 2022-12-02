using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Input
{
    public class InputActionHandler : MonoBehaviour
    {
        [SerializeField] private InputActionReference inputActionReference;
        
        [Space(10)]
        
        [SerializeField] private InputEvent startedInputEvent;
        [SerializeField] private InputEvent performedInputEvent;
        [SerializeField] private InputEvent canceledInputEvent;


        private void OnEnable()
        {
            inputActionReference.action.Enable();
            
            inputActionReference.action.started += startedInputEvent.Raise;
            inputActionReference.action.performed += performedInputEvent.Raise;
            inputActionReference.action.canceled += canceledInputEvent.Raise;
        }

        private void OnDisable()
        {
            inputActionReference.action.Disable();

            inputActionReference.action.started -= startedInputEvent.Raise;
            inputActionReference.action.performed -= performedInputEvent.Raise;
            inputActionReference.action.canceled -= canceledInputEvent.Raise;
        }

        public void SetStartedInputEvent(InputEvent inputEvent)
        {
            startedInputEvent = inputEvent;
        }
        
        public void SetPerformedInputEvent(InputEvent inputEvent)
        {
            performedInputEvent = inputEvent;
        }
        
        public void SetCanceledInputEvent(InputEvent inputEvent)
        {
            canceledInputEvent = inputEvent;
        }
    }
}
