using System;
using Core.Debug;
using Core.Events;
using Core.Sets;
using Core.UpdateManager;
using Unity.Mathematics;
using UnityEngine;

namespace PaperBoy.Bike
{
    public class BikeDamageController : UpdateableBehaviour
    {
        [SerializeField] private BikeDamageControllerSO bikeDamageControllerSO;
        [SerializeField] private RuntimeGameEventListener damageEventListener;
        [SerializeField] private SkinnedMeshRenderer[] skinnedMeshRenderers;

        private int _currentHealth;
        private float _cooldownTimer;

        private const int MAX_HEALTH = 100;
    

        private void OnValidate()
        {
            skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
            {
                skinnedMeshRenderer.SetBlendShapeWeight(0, 0);
            }
        }

        protected override void Awake()
        {
            base.Awake();

            _currentHealth = MAX_HEALTH;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            damageEventListener.Subscribe(Damage);
        }

        protected override void OnDisable()
        {
            damageEventListener.Unsubscribe(Damage);
            
            base.OnDisable();
        }

        public override void ManagedUpdate()
        {
            base.ManagedUpdate();

            if (_cooldownTimer > 0)
            {
                _cooldownTimer -= Time.deltaTime;
            }
        }

        private void Damage()
        {
            if (_cooldownTimer > 0)
            {
                return;
            }

            _cooldownTimer = bikeDamageControllerSO.Cooldown;
            
            _currentHealth -= bikeDamageControllerSO.HitDamage;
            int blendValue = math.clamp(MAX_HEALTH - _currentHealth, 0, MAX_HEALTH);
            foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
            {
                skinnedMeshRenderer.SetBlendShapeWeight(0, blendValue);
            }
        }
    }
}
