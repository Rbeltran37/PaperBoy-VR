using Core.Events;
using PaperBoy.ObjectPool;
using PaperBoy.Sets;
using UnityEngine;

namespace PaperBoy.Effects
{
    public class EffectsHandler : MonoBehaviour
    {
        [SerializeField] private RuntimeGameEventListener spawnEventListener;
        [SerializeField] private RuntimeGameEventListener despawnEventListener;
        [SerializeField] private ObjectPoolSet objectPoolSet;
        [SerializeField] private EffectsSO effectsSO;

        private Transform _transform;
        private ObjectPool.ObjectPool _objectPool;
        private PoolableObject _poolableObject;
        private EffectsManager _effectsManager;

        private void Awake()
        {
            _transform = transform;
        }

        private void Start()
        {
            _objectPool = objectPoolSet.Items[0];
        }

        private void OnEnable()
        {
            spawnEventListener.Subscribe(AttemptSpawnEffects);
            
            if (despawnEventListener)
            {
                despawnEventListener.Subscribe(AttemptDespawnEffects);
            }
        }

        private void OnDisable()
        {
            spawnEventListener.Unsubscribe(AttemptSpawnEffects);
            
            if (despawnEventListener)
            {
                despawnEventListener.Unsubscribe(AttemptDespawnEffects);
            }
        }

        private void AttemptSpawnEffects()
        {
            _poolableObject = _objectPool.GetPooledObject();
            _effectsManager = _poolableObject.GetComponent<EffectsManager>();
            Transform tempParent = effectsSO.IsParentedToHandler ? _transform : null;
            
            _effectsManager.SpawnEffects(effectsSO, _transform.position, _transform.rotation, tempParent);
        }

        private void AttemptDespawnEffects()
        {
            _effectsManager.Despawn();
        }
    }
}