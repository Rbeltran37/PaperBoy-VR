using System;
using Core.Debug;
using UnityEngine;

namespace Core.Singleton
{
    //https://blog.mzikmund.com/2019/01/a-modern-singleton-in-unity/
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static Lazy<T> _lazyInstance = new Lazy<T>(CreateSingleton);
    
        public static T Instance => _lazyInstance.Value;
        
        private static readonly string _name = $"{typeof(T).Name} (Singleton)";


        private static T CreateSingleton()
        {
            GameObject ownerObject = new GameObject(_name);
            T instance = ownerObject.AddComponent<T>();
            DontDestroyOnLoad(ownerObject);
            CustomLogger.EditorOnlyInfo(nameof(CreateSingleton), $"{typeof(T).Name}");
            return instance;
        }
    }
}
