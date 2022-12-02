using UnityEngine;

namespace PaperBoy.ObjectPool
{
    public class PoolableObject : MonoBehaviour
    {
        [HideInInspector] [SerializeField] protected GameObject ThisGameObject;
        [HideInInspector] [SerializeField] protected Transform ThisTransform;

        private Transform _objectPoolTransform;
        private ObjectPool _objectPool;
        

        protected virtual void OnValidate()
        {
            ThisGameObject = gameObject;
            ThisTransform = transform;
        }

        protected virtual void Awake()
        {
            ThisGameObject.SetActive(false);
        }

        public void SetObjectPool(ObjectPool objectPool)
        {
            _objectPool = objectPool;
            _objectPoolTransform = _objectPool.transform;
        }

        public virtual void Spawn(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            Transform spawnParent = parent ? parent : _objectPoolTransform;
            ThisTransform.SetParent(spawnParent);
            ThisTransform.SetPositionAndRotation(position, rotation);
            ThisGameObject.SetActive(true);
        }

        public virtual void Despawn()
        {
            ThisTransform.SetParent(_objectPoolTransform);
            _objectPool.Pool(this);
            ThisGameObject.SetActive(false);
        }
    }
}
