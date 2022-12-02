using System;
using System.Collections.Generic;
using Core.Debug;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Events
{
    public class DebugGameEventListener : MonoBehaviour
    {
        public List<GameEventListener> GameEventListeners = new List<GameEventListener>();


        private void OnEnable()
        {
            AddCallbackAtRuntime();
        }

        private void OnDisable()
        {
            UnsubscribeAtRuntime();
            
        }

        private void UnsubscribeAtRuntime()
        {
            foreach (var gameEventListener in GameEventListeners)
            {
                if (gameEventListener is RuntimeGameEventListener runtimeGameEventListener)
                {
                    runtimeGameEventListener.Unsubscribe(PublicCall);
                    runtimeGameEventListener.Unsubscribe(PositionRandomly);
                    runtimeGameEventListener.Unsubscribe(RandomRemove);
                }
                else if (gameEventListener is EditorGameEventListener editorGameEventListener)
                {
                    //Cannot unsubscribe at runtime
                }
            }
        }

        public void AddGameEventListener()
        {
            GameEventListener gameEventListener = GetComponentInParent<GameEventListener>();
            if (!gameEventListener)
            {
                return;
            }
            
            if (!GameEventListeners.Contains(gameEventListener))
            {
                GameEventListeners.Add(gameEventListener);
            }
        }

        private void AddCallbackAtRuntime()
        {
            foreach (var gameEventListener in GameEventListeners)
            {
                if (gameEventListener is RuntimeGameEventListener runtimeGameEventListener)
                {
                    runtimeGameEventListener.Subscribe(PublicCall);
                    runtimeGameEventListener.Subscribe(PositionRandomly);
                    runtimeGameEventListener.Subscribe(RandomRemove);
                }
            }
        }

        public void PublicCall()
        {
            CustomLogger.EditorOnlyInfo(nameof(PublicCall));
        }
        
        private const float POSITION_RANGE = 100;

        public void PositionRandomly()
        {
            float x = Random.Range(-POSITION_RANGE, POSITION_RANGE);
            float y = Random.Range(-POSITION_RANGE, POSITION_RANGE);
            float z = Random.Range(-POSITION_RANGE, POSITION_RANGE);
            transform.position = new Vector3(x, y, z);
        }

        public void RandomRemove()
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
                        DebugGameEventListener otherDebugUpdateable = otherTransform.GetComponent<DebugGameEventListener>();
                        otherDebugUpdateable.RandomRemove();
                    }

                    break;
                }
                default:
                {
                    throw new Exception("Debug Exception");
                }
            }
        }
    }
}
