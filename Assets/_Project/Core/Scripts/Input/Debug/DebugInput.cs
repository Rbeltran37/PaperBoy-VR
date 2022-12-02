using System.Collections;
using System.Collections.Generic;
using Core.Debug;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Input
{
    public class DebugInput : MonoBehaviour
    {
        public void DebugButton(InputAction.CallbackContext callbackContext)
        {
            CustomLogger.Debug(nameof(DebugButton), $"action={callbackContext.action} valueType={callbackContext.valueType} phase={callbackContext.phase}");
        }
        
    }
}
