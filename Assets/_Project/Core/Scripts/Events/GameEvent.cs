// ----------------------------------------------------------------------------
// Inspired by:
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Core.Debug;
using UnityEngine;

namespace Core.Events
{
    [CreateAssetMenu(fileName = "GameEvent_", menuName = "ScriptableObjects/Core/Events/GameEvent", order = 1)]
    public class GameEvent : ScriptableObject
    {
        // The list of listeners that this event will notify if it is raised.
        private List<GameEventListener> _gameEventListeners = new List<GameEventListener>();
        

        public void Raise()
        {
            for (int i = _gameEventListeners.Count - 1; i >= 0; i--)
            {
                GameEventListener currentGameEventListener = _gameEventListeners[i];
                if (currentGameEventListener == null)
                {
                    continue;
                }
                
                try
                {
                    currentGameEventListener.OnEventRaised();
                }
                catch (Exception exception)
                {
                    CustomLogger.EditorOnlyError(nameof(Raise), $"Exception in {nameof(currentGameEventListener)}={currentGameEventListener} in {_gameEventListeners}. Unregistering...", currentGameEventListener);
                    UnregisterListener(currentGameEventListener);
                    throw exception;
                }
            }
        }

        public void RegisterListener(GameEventListener listener)
        {
            if (_gameEventListeners.Contains(listener))
            {
                CustomLogger.EditorOnlyWarning(nameof(RegisterListener), $"{nameof(_gameEventListeners)} already contains {listener}. Cannot register.");
                return;
            }
            
            _gameEventListeners.Add(listener);
        }

        public void UnregisterListener(GameEventListener listener)
        {
            if (!_gameEventListeners.Contains(listener))
            {
                CustomLogger.EditorOnlyWarning(nameof(UnregisterListener), $"{nameof(_gameEventListeners)} doesn't contains {listener}. Cannot unregister.");
                return;
            }
            
            _gameEventListeners.Remove(listener);
        }
    }
}
