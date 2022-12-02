using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Variables
{
    [CreateAssetMenu(fileName = "StringVariable_", menuName = "ScriptableObjects/Core/Variables/StringVariable", order = 1)]
    public class StringVariable : ScriptableObject
    {
        [SerializeField]
        private string value = "";

        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }
}
