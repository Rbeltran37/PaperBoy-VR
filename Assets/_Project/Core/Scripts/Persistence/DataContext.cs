using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.Persistence
{
    public abstract class DataContext : ScriptableObject
    {
        public GameData gameData;

        public abstract Task Load();
        public abstract Task Save();
        
        public List<T> Set<T>()
        {
            foreach (List<DataObject> list in gameData.dataObjectLists)
            {
                if (list.Count > 0 && typeof(T) == list[0].GetType())
                {
                    return list as List<T>;
                }
            }

            return null;
        }
    }
}
