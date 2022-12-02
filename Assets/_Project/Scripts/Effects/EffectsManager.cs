using System.Collections.Generic;
using Core.Sets;
using Core.UpdateManager;
using PaperBoy.ObjectPool;
using UnityEngine;

namespace PaperBoy.Effects
{
    public class EffectsManager : PoolableObject, IUpdateable
    {
        [SerializeField] private List<UpdateRuntimeSet> updateRuntimeSets;
        [SerializeField] private Effects[] effects;
        
        private float _lifetime;
        private GameObject _currentEffectsGameObject;
        private Effects _currentEffects;
        private EffectsSO _currentEffectsSO;
        private Dictionary<EffectsSO, Effects> _effectsDictionary = new Dictionary<EffectsSO, Effects>();

        
        protected override void OnValidate()
        {
            base.OnValidate();

            effects = GetComponentsInChildren<Effects>();
        }

        protected override void Awake()
        {
            base.Awake();
            
            foreach (var updateRuntimeSet in updateRuntimeSets)
            {
                updateRuntimeSet.Add(this);
            }

            foreach (Effects effect in effects)
            {
                _effectsDictionary.Add(effect.EffectsSO, effect);
                effect.gameObject.SetActive(false);
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

        public virtual void SmartUpdate()
        {
            _lifetime += Time.deltaTime;
            if (_lifetime > _currentEffectsSO.Lifetime)
            {
                Despawn();
            }
        }

        public override void Spawn(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            base.Spawn(position, rotation, parent);

            _lifetime = 0;
        }

        public override void Despawn()
        {
            _currentEffectsGameObject.SetActive(false);
            
            base.Despawn();
        }

        public void SpawnEffects(EffectsSO effectsSO, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            _currentEffects = _effectsDictionary[effectsSO];
            _currentEffectsGameObject = _currentEffects.gameObject;
            _currentEffectsSO = effectsSO;
            
            _currentEffectsGameObject.SetActive(true);
            
            Spawn(position, rotation, parent);
        }
    }
}
