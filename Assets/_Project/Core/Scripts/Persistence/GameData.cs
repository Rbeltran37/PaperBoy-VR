using System.Collections.Generic;
using UnityEngine;

namespace Core.Persistence
{
    public abstract class GameData : ScriptableObject, ISerializationCallbackReceiver
    {
        public List<List<DataObject>> dataObjectLists;
        
        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
            InitializeValues();
        }

        protected abstract void InitializeValues();
    }
}
