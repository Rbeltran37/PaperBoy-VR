// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using UnityEngine;

namespace Core.Variables
{
    [CreateAssetMenu(fileName = "IntVariable_", menuName = "ScriptableObjects/Core/Variables/IntVariable", order = 1)]
    public class IntVariable : ScriptableObject
    {
        public int Value;
    }
}