using System;
using Core.UpdateManager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PaperBoy.Bike
{
    public class BikeLocomotionAnimator : UpdateableBehaviour
    {
        [SerializeField] private BikeLocomotionAnimatorSO bikeLocomotionAnimatorSO;
        [SerializeField] private Transform handleBars;
        [SerializeField] private Transform rotationPivot;
        
        private float _lateralValue;
        private float _handleBarLerpProgress;
        private float _bikeTurnLerpProgress;
        private float _bikeLeanLerpProgress;
        private Quaternion _centeredHandleBarsRotation;
        private Quaternion _centeredPivotRotation;

        private readonly Vector3 _turnAxis = new Vector3(0, 90, 0);        //Fully turned to the right
        private readonly Vector3 _leanAxis = new Vector3(0, 0, -90);        //Fully leaned to the right

        private Vector3 HandleBarTurnAmount => _turnAxis * (_lateralValue * bikeLocomotionAnimatorSO.HandleBarTurnMultiplier);
        private Vector3 BikeTurnAmount => _turnAxis * (_lateralValue * bikeLocomotionAnimatorSO.BikeTurnMultiplier);
        private Vector3 BikeLeanAmount => _leanAxis * (_lateralValue * bikeLocomotionAnimatorSO.BikeLeanMultiplier);


        protected override void Awake()
        {
            base.Awake();

            _centeredHandleBarsRotation = handleBars.localRotation;
            _centeredPivotRotation = rotationPivot.localRotation;
        }

        public override void ManagedLateUpdate()
        {
            var percentage = GetLerpPercentage(ref _handleBarLerpProgress, bikeLocomotionAnimatorSO.HandleBarTurnLerpTime);
            TurnHandleBars(percentage);

            percentage = GetLerpPercentage(ref _bikeTurnLerpProgress, bikeLocomotionAnimatorSO.BikeTurnLerpTime);
            TurnBike(percentage);
            
            percentage = GetLerpPercentage(ref _bikeLeanLerpProgress, bikeLocomotionAnimatorSO.BikeLeanLerpTime);
            LeanBike(percentage);
        }

        private float GetLerpPercentage(ref float currentLerpTime, float lerpTime)
        {
            //increment timer once per frame
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
            }

            //lerp
            float percentage = currentLerpTime / lerpTime;
            return percentage;
        }

        private void TurnHandleBars(float percentage)
        {
            Quaternion targetRotation = _centeredHandleBarsRotation * Quaternion.Euler(HandleBarTurnAmount);
            handleBars.localRotation = Quaternion.Lerp(handleBars.localRotation, targetRotation, percentage);
        }
        
        private void TurnBike(float percentage)
        {
            Quaternion targetRotation = _centeredPivotRotation * Quaternion.Euler(BikeTurnAmount);
            rotationPivot.localRotation = Quaternion.Lerp(rotationPivot.localRotation, targetRotation, percentage);
        }
        
        private void LeanBike(float percentage)
        {
            Quaternion targetRotation = _centeredPivotRotation * Quaternion.Euler(BikeLeanAmount);
            rotationPivot.localRotation = Quaternion.Lerp(rotationPivot.localRotation, targetRotation, percentage);
        }

        //Called by input
        public void SetTurnValue(InputAction.CallbackContext callbackContext)
        {
            Vector2 axisValue = callbackContext.ReadValue<Vector2>();
            float x = Math.Abs(axisValue.x);
            if (x < bikeLocomotionAnimatorSO.XrLocomotionSO.LateralLocomotionOnSensitivity)
            {
                _lateralValue = 0;
                return;
            }

            _lateralValue = axisValue.x;
            
            _handleBarLerpProgress = 0;
            _bikeTurnLerpProgress = 0;
            _bikeLeanLerpProgress = 0;
        }
    }
}
