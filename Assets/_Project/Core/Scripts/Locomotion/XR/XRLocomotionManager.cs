using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Locomotion.XR
{
    public class XRLocomotionManager : MonoBehaviour
    {
        [SerializeField] private Transform playArea;
        [SerializeField] private Transform headset;
        [SerializeField] private List<XRMovementSolution> movementSolutions;
        [SerializeField] private List<XRTurningSolution> turningSolutions;
        
        private XRTurningSolution _turningSolution;
        private XRMovementSolution _movementSolution;
        private List<float> _turnSpeeds;
        private List<float> _movementSpeeds;


        private void Start()
        {
            SetTurningSolution(0);
            SetMovementSolution(0);

            foreach (var turningSolution in turningSolutions)
            {
                turningSolution.Initialize(playArea, headset);
            }

            foreach (var movementSolution in movementSolutions)
            {
                movementSolution.Initialize(playArea, headset);
            }
        }
        
        public void SetTurningSolution(int indexToActivate)
        {
            if (indexToActivate >= turningSolutions.Count)
            {
                return;
            }
            
            _turningSolution = turningSolutions[indexToActivate];
            _turnSpeeds = _turningSolution.GetTurnSpeeds();
            
            float turnSpeed = _turnSpeeds[indexToActivate];
            _turningSolution.SetCurrentTurnSpeed(turnSpeed);
        }

        public void SetMovementSolution(int indexToActivate)
        {
            if (indexToActivate >= movementSolutions.Count)
            {
                return;
            }
            
            _movementSolution = movementSolutions[indexToActivate];
            _movementSpeeds = _movementSolution.GetMovementSpeeds();

            float movementSpeed = _movementSpeeds[indexToActivate];
            _movementSolution.SetCurrentMovementSpeed(movementSpeed);
        }
        
        public void Move(InputAction.CallbackContext callbackContext)
        {
            if (!_movementSolution)
            {
                return;
            }
            
            _movementSolution.ReadInput(callbackContext);
        }

        public void Turn(InputAction.CallbackContext callbackContext)
        {
            if (!_turningSolution)
            {
                return;
            }
            
            _turningSolution.ReadInput(callbackContext);
        }
    }
}
