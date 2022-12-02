using Core.Debug;
using Core.Events;
using UnityEngine;

namespace PaperBoy.Bike
{
    public class BikeCollider : MonoBehaviour
    {
        [SerializeField] private Transform parent;
        [SerializeField] private RuntimeGameEventListener damageEventListener;
        [SerializeField] private RuntimeGameEventListener fillBasketEventListener;
        

        private void Awake()
        {
            transform.parent = parent;
        }

        private void OnCollisionEnter(Collision other)
        {
            NewspaperStackCollider newspaperStackCollider = other.transform.GetComponent<NewspaperStackCollider>();
            if (newspaperStackCollider)
            {
                fillBasketEventListener.OnEventRaised();
                newspaperStackCollider.Collect();
                CustomLogger.Debug(nameof(OnCollisionEnter), $"Collect", other.gameObject);
                return;
            }
            
            damageEventListener.OnEventRaised();
        }
    }
}
