using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

namespace Core.Events
{
    [CustomEditor(typeof(GameEventListener))]
    [CanEditMultipleObjects]
    public class GameEventListenerEditor : Editor
    {
        protected List<Action> Callbacks;

        private const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.NonPublic;

        
        protected virtual void OnEnable()
        {
            GameEventListener gameEventListener = target as GameEventListener;

            Type type = typeof(GameEventListener);
            FieldInfo fieldInfo = type.GetField(nameof(Callbacks), BINDING_FLAGS);
            Callbacks = fieldInfo.GetValue(gameEventListener) as List<Action>;
        }
    }
}
