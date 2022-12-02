using System.Collections.Generic;
using Core.Sets;
using UnityEngine;

namespace Core.UpdateManager
{
    public abstract class UpdateableBehaviour : MonoBehaviour, IUpdateable
    {
        [SerializeField] private List<UpdateRuntimeSet> updateRuntimeSets;


        protected virtual void Awake()
        {
            foreach (var updateRuntimeSet in updateRuntimeSets)
            {
                updateRuntimeSet.Add(this);
            }
        }

        protected virtual void OnEnable()
        {
            foreach (var updateRuntimeSet in updateRuntimeSets)
            {
                updateRuntimeSet.Add(this);
            }
        }

        protected virtual void OnDisable()
        {
            foreach (var updateRuntimeSet in updateRuntimeSets)
            {
                updateRuntimeSet.Remove(this);
            }
        }
        
        public bool IsValid()
        {
            return isActiveAndEnabled;
        }

        public virtual void ManagedUpdate() {}
        public virtual void ManagedFixedUpdate() {}
        public virtual void ManagedLateUpdate() {}
        public virtual void SmartUpdate() {}
    }
}
