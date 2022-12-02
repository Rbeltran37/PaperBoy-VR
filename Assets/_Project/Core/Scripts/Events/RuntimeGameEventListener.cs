using System;
using Core.Debug;

namespace Core.Events
{
    public class RuntimeGameEventListener : GameEventListener
    {
        public void Subscribe(Action callback)
        {
            if (Callbacks.Contains(callback))
            {
                CustomLogger.EditorOnlyWarning(nameof(Subscribe), $"{nameof(Callbacks)} already contains {callback}. Cannot subscribe.");
                return;
            }
            
            Callbacks.Add(callback);
        }
        
        public void Unsubscribe(Action callback)
        {
            if (!Callbacks.Contains(callback))
            {
                CustomLogger.EditorOnlyWarning(nameof(Unsubscribe), $"{nameof(Callbacks)} doesn't contain {callback}. Cannot unsubscribe.");
                return;
            }
            
            Callbacks.Remove(callback);
        }
        
        public override void OnEventRaised()
        {
            for (int i = Callbacks.Count - 1; i >= 0; i--)
            {
                Action currentCallback = Callbacks[i];
                if (currentCallback == null)
                {
                    continue;
                }

                try
                {
                    currentCallback.Invoke();
                }
                catch (Exception exception)
                {
                    CustomLogger.EditorOnlyError(nameof(OnEventRaised), $"Exception in {nameof(currentCallback)}={currentCallback} in {nameof(Callbacks)}. Unsubscribing...", currentCallback.Target as UnityEngine.Object);
                    Unsubscribe(currentCallback);
                    throw exception;
                }
            }
        }
    }
}
