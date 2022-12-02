using System;
using System.Collections.Generic;
using System.Diagnostics;
using Core.Debug;
using UnityEngine;
using Core.Sets;
using Object = UnityEngine.Object;

namespace Core.UpdateManager
{
    public class UpdateManager : MonoBehaviour
    {
        [SerializeField] private UpdateRuntimeSet fixedUpdateRuntimeSet;
        [SerializeField] private UpdateRuntimeSet updateRuntimeSet;
        [SerializeField] private UpdateRuntimeSet lateUpdateRuntimeSet;
        [SerializeField] private UpdateRuntimeSet smartUpdateRuntimeSet;

        private int _currentStartIndex;
        private IUpdateable _currentUpdateable;
        private Object _currentUpdateableObject;
        private List<IUpdateable> _currentUpdateables;
        private IUpdateable[] _currentUpdateablesArray = new IUpdateable[ARRAY_SIZE];
        private Stopwatch _stopwatch = new Stopwatch();
        private long _currentTicks;
        private long _throwawayTicks;
        private long[] _ticks = new long[FRAMES_TO_AVERAGE];
        private int _tickIndex;
        private long _sumTicks;
        private long _averageTicksPerFrame;

        private const int ARRAY_SIZE = 1000;
        private const int FRAMES_TO_AVERAGE = 50;


        private void FixedUpdate()
        {
            _stopwatch.Restart();
            
            _currentUpdateables = fixedUpdateRuntimeSet.Items;
            _currentUpdateables.CopyTo(_currentUpdateablesArray);
            _currentStartIndex = _currentUpdateables.Count - 1;
            for (int i = _currentStartIndex; i >= 0; i--)
            {
                _currentUpdateable = _currentUpdateablesArray[i];
                if (_currentUpdateable == null || !_currentUpdateable.IsValid())
                {
                    continue;
                }

                try
                {
                    _currentUpdateable.ManagedFixedUpdate();
                }
                catch (Exception exception)
                {
#if UNITY_EDITOR
                    _currentUpdateableObject = _currentUpdateable as Object;
                    CustomLogger.EditorOnlyError(nameof(FixedUpdate),
                        $"{exception.Message} | {nameof(_currentUpdateable)}={_currentUpdateable} in {fixedUpdateRuntimeSet}. Removing...", 
                        _currentUpdateableObject);
#endif
                    fixedUpdateRuntimeSet.Remove(_currentUpdateable);
                    throw;
                }
            }
        }
        
        private void Update()
        {
            _currentUpdateables = updateRuntimeSet.Items;
            _currentUpdateables.CopyTo(_currentUpdateablesArray);
            _currentStartIndex = _currentUpdateables.Count - 1;
            for (int i = _currentStartIndex; i >= 0; i--)
            {
                _currentUpdateable = _currentUpdateablesArray[i];
                if (_currentUpdateable == null || !_currentUpdateable.IsValid())
                {
                    continue;
                }

                try
                {
                    _currentUpdateable.ManagedUpdate();
                }
                catch (Exception exception)
                {
#if UNITY_EDITOR
                    _currentUpdateableObject = _currentUpdateable as Object;
                    CustomLogger.EditorOnlyError(nameof(Update),
                        $"{exception.Message} | {nameof(_currentUpdateable)}={_currentUpdateable} in {updateRuntimeSet}. Removing...", 
                        _currentUpdateableObject);
#endif
                    updateRuntimeSet.Remove(_currentUpdateable);
                    throw;
                }
            }
        }
        
        private void LateUpdate()
        {
            _currentUpdateables = lateUpdateRuntimeSet.Items;
            _currentUpdateables.CopyTo(_currentUpdateablesArray);
            _currentStartIndex = _currentUpdateables.Count - 1;
            for (int i = _currentStartIndex; i >= 0; i--)
            {
                _currentUpdateable = _currentUpdateablesArray[i];
                if (_currentUpdateable == null || !_currentUpdateable.IsValid())
                {
                    continue;
                }

                try
                {
                    _currentUpdateable.ManagedLateUpdate();
                }
                catch (Exception exception)
                {
#if UNITY_EDITOR
                    _currentUpdateableObject = _currentUpdateable as Object;
                    CustomLogger.EditorOnlyError(nameof(LateUpdate),
                        $"{exception.Message} | {nameof(_currentUpdateable)}={_currentUpdateable} in {lateUpdateRuntimeSet}. Removing...", 
                        _currentUpdateableObject);
#endif
                    lateUpdateRuntimeSet.Remove(_currentUpdateable);
                    throw;
                }
            }
            
            SmartUpdate();
        }

        private void SmartUpdate()
        {
            //Get average ticks per frame
            _currentTicks = _stopwatch.ElapsedTicks;
            _throwawayTicks = _ticks[_tickIndex];
            _ticks[_tickIndex++] = _currentTicks;
            if (_tickIndex >= FRAMES_TO_AVERAGE)
            {
                _tickIndex = 0;
            }

            _sumTicks -= _throwawayTicks;
            _sumTicks += _currentTicks;
            _averageTicksPerFrame = _sumTicks / FRAMES_TO_AVERAGE;

            //If high-usage frame, return
            if (_currentTicks >= _averageTicksPerFrame)
            {
                return;
            }
            
            //Iterate through SmartUpdate Set
            _currentUpdateables = smartUpdateRuntimeSet.Items;
            _currentUpdateables.CopyTo(_currentUpdateablesArray);
            _currentStartIndex = _currentUpdateables.Count - 1;
            for (int i = _currentStartIndex; i >= 0 && _currentTicks < _averageTicksPerFrame; i--)       //Check current frame usage
            {
                _currentUpdateable = _currentUpdateablesArray[i];
                if (_currentUpdateable == null || !_currentUpdateable.IsValid())
                {
                    continue;
                }

                try
                {
                    _currentUpdateable.SmartUpdate();
                }
                catch (Exception exception)
                {
#if UNITY_EDITOR
                    _currentUpdateableObject = _currentUpdateable as Object;
                    CustomLogger.EditorOnlyError(nameof(SmartUpdate),
                        $"{exception.Message} | {nameof(_currentUpdateable)}={_currentUpdateable} in {smartUpdateRuntimeSet}. Removing...", 
                        _currentUpdateableObject);
#endif
                    smartUpdateRuntimeSet.Remove(_currentUpdateable);
                    throw;
                }
                    
                _currentTicks = _stopwatch.ElapsedTicks;        //Check if frame still qualifies as low-usage frame
            }
        }
    }
}
