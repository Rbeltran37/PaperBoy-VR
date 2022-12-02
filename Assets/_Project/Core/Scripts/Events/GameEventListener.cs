// ----------------------------------------------------------------------------
// Inspired by:
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Events
{
    public abstract class GameEventListener : MonoBehaviour
    {
        [Tooltip("Event to register with.")]
        [SerializeField] private GameEvent gameEvent;

        protected List<Action> Callbacks = new List<Action>();


        protected virtual void OnEnable()
        {
            gameEvent.RegisterListener(this);
        }

        protected virtual void OnDisable()
        {
            gameEvent.UnregisterListener(this);
        }

        public abstract void OnEventRaised();
    }
}