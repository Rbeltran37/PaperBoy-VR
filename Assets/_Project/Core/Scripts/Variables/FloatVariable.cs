// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using System;
using UnityEngine;

namespace Core.Variables
{
    [CreateAssetMenu(fileName = "FloatVariable_", menuName = "ScriptableObjects/Core/Variables/FloatVariable", order = 1)]
    public class FloatVariable : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private float initialValue;
        
        [NonSerialized] public float RuntimeValue;


        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
            RuntimeValue = initialValue;
        }
    }
}
