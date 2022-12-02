using System;
using Core.Debug;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.UpdateManager
{
    public class DebugUpdateable : UpdateableBehaviour
    {
        private bool _runInFixedUpdate;
        private bool _runInUpdateUpdate;
        private bool _runInLateUpdate;
        private bool _runInSmartUpdate;
        private Action _currentCall;
        
        private const float POSITION_RANGE = 100;


        public void PublicCall()
        {
            RunInUpdates();
            _currentCall = PublicCallInUpdates;
        }

        private void PublicCallInUpdates()
        {
            CustomLogger.Debug(nameof(PublicCall), $"DEBUG", this);
        }
        
        public void PositionRandomly()
        {
            RunInUpdates();
            _currentCall = PositionRandomlyInUpdates;
        }

        private void PositionRandomlyInUpdates()
        {
            float x = Random.Range(-POSITION_RANGE, POSITION_RANGE);
            float y = Random.Range(-POSITION_RANGE, POSITION_RANGE);
            float z = Random.Range(-POSITION_RANGE, POSITION_RANGE);
            transform.position = new Vector3(x, y, z);
        }

        public void RandomRemove()
        {
            RunInUpdates();
            _currentCall = RandomRemoveInUpdates;
        }

        private void RandomRemoveInUpdates()
        {
            int randomInt = Random.Range(0, 2);
            if (randomInt != 1)
            {
                return;
            }
            
            randomInt = Random.Range(0, 5);
            switch (randomInt)
            {
                case 0:
                {
                    gameObject.SetActive(false);
                    break;
                }
                case 1:
                {
                    enabled = false;
                    break;
                }
                case 2:
                {
                    Destroy(gameObject);
                    break;
                }
                case 3:
                {
                    Transform parent = transform.parent;
                    if (parent)
                    {
                        int childCount = parent.childCount;
                        randomInt = Random.Range(0, childCount);
                        Transform otherTransform = parent.GetChild(randomInt);
                        DebugUpdateable otherDebugUpdateable = otherTransform.GetComponent<DebugUpdateable>();
                        if (otherDebugUpdateable.IsValid())
                        {
                            otherDebugUpdateable.RandomRemove();
                        }
                    }

                    break;
                }
                default:
                {
                    throw new Exception("Debug Exception");
                }
            }
        }

        private void RunInUpdates()
        {
            _runInFixedUpdate = true;
            _runInUpdateUpdate = true;
            _runInLateUpdate = true;
            _runInSmartUpdate = true;
        }

        public override void ManagedFixedUpdate()
        {
            if (!_runInFixedUpdate)
            {
                return;
            }

            _runInFixedUpdate = false;
            _currentCall.Invoke();
        }

        public override void ManagedUpdate()
        {
            if (!_runInUpdateUpdate)
            {
                return;
            }

            _runInUpdateUpdate = false;
            _currentCall.Invoke();
        }

        public override void ManagedLateUpdate()
        {
            if (!_runInLateUpdate)
            {
                return;
            }

            _runInLateUpdate = false;
            _currentCall.Invoke();
        }

        public override void SmartUpdate()
        {
            if (!_runInSmartUpdate)
            {
                return;
            }

            _runInSmartUpdate = false;
            _currentCall.Invoke();
        }
    }
}
