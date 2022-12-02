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

namespace Core.Sets
{
    public abstract class RuntimeSet<T> : ScriptableObject
    {
        [NonSerialized] public readonly List<T> Items = new List<T>();


        public void Add(T item)
        {
#if UNITY_EDITOR
            if (Items.Contains(item))
            {
                CustomLogger.EditorOnlyWarning(nameof(Add), $"{nameof(Items)} already contains {item}. Cannot add.");
                return;
            }
#endif
            Items.Add(item);
        }

        public void Remove(T item)
        {
#if UNITY_EDITOR
            if (!Items.Contains(item))
            {
                CustomLogger.EditorOnlyWarning(nameof(Remove), $"{nameof(Items)} doesn't contain {item}. Cannot Remove.");
                return;
            }
#endif
            Items.Remove(item);
        }
    }
}
