using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grab : MonoBehaviour
{
    [SerializeField] private List<Collider> newspaperCollidersList = new List<Collider>();

    private bool _isGrabbing;
    private bool _isHoldingNewspaper;
    private int _handVelocityListIterator;
    private Vector3 _lastPosition;
    private Vector3 _handVelocity;
    private Vector3 _handVelocityAverage;
    private List<Vector3> _handVelocityList = new List<Vector3>();
    private Transform _newspaperTransform;
    private Rigidbody _newspaperRigidbody;
    private Newspaper _newspaper;

    public Action GrabbedNewspaper;
    public Action LaunchedNewspaper;

    private const int MAX_LIST_SIZE = 5;
    private const float Y_AXIS_OFFSET = 0.649f;


    private void Update()
    {
        if (!_isHoldingNewspaper)
            return;

        if (_lastPosition == Vector3.zero)
        {
            _lastPosition = transform.position;
            return;
        }

        CalculateVelocity();
        
        _lastPosition = transform.position;

        AddVelocityToList();
    }

    private void CalculateVelocity()
    {
        _handVelocity = (transform.position - _lastPosition) / Time.deltaTime;
    }

    private void AddVelocityToList()
    {
        if (_handVelocityList.Count < MAX_LIST_SIZE)
        {
            _handVelocityList.Add(_handVelocity);
        }
        
        else
        {
            if (_handVelocityListIterator >= _handVelocityList.Count)
            {
                _handVelocityListIterator = 0;
            }

            _handVelocityList[_handVelocityListIterator] = _handVelocity;
            _handVelocityListIterator++;
        }
    }
    
    public void ReadGrabInput(InputAction.CallbackContext callbackContext)
    {
        AttemptGrab();
    }
        
    private void AttemptGrab()
    {
        _isGrabbing = true;
    }
    
    public void ReadThrowInput(InputAction.CallbackContext callbackContext)
    {
        AttemptThrow();
    }
    
    private void AttemptThrow()
    {
        _isGrabbing = false;

        if (_isHoldingNewspaper)
        {
            ThrowNewspaper();
        }
    }

    private void ThrowNewspaper()
    {
        CalculateVelocityAverage();
        LaunchNewspaper();
        ResetVariables();
    }

    private void CalculateVelocityAverage()
    {
        foreach (var t in _handVelocityList)
        {
            _handVelocityAverage += t;
        }

        _handVelocityAverage.x /= _handVelocityList.Count;
        _handVelocityAverage.y /= _handVelocityList.Count;
        _handVelocityAverage.z /= _handVelocityList.Count;
    }

    private void LaunchNewspaper()
    {
        _newspaper.Throw(_handVelocityAverage);
        
        LaunchedNewspaper?.Invoke();
    }
    
    private void ResetVariables()
    {
        _isHoldingNewspaper = false;
        _newspaperTransform = null;
        _newspaperRigidbody = null;
        _handVelocityList.Clear();
        _lastPosition = Vector3.zero;
        _handVelocity = Vector3.zero;
        _handVelocityAverage = Vector3.zero;
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (CanBeGrabbed(other))
        {
            AttemptGrabNewspaper(other);
        }
    }

    private bool CanBeGrabbed(Collider other)
    {
        return _isGrabbing && 
               !_isHoldingNewspaper && 
               other.GetComponent<Newspaper>();        //KB : Using GetComponent at runtime instead of searching collider list
    }

    private void AttemptGrabNewspaper(Collider other)
    {
        AssignNewspaperVariables(other);
        
        GrabNewspaper();
        
        _isHoldingNewspaper = true;
    }

    private void AssignNewspaperVariables(Collider other)
    {
        _newspaperRigidbody = other.attachedRigidbody;
        _newspaperTransform = other.transform;
        _newspaper = other.GetComponent<Newspaper>();
    }
    
    private void GrabNewspaper()
    {
        _newspaperRigidbody.isKinematic = true;
        _newspaperRigidbody.velocity = Vector3.zero;
        _newspaperRigidbody.angularVelocity = Vector3.zero;
        _newspaperTransform.parent = gameObject.transform;
        _newspaperTransform.localPosition = new Vector3(0f, 0, 0f);        //KB : Removed Y offset
        _newspaperTransform.localRotation = quaternion.Euler(0f, 0f, 0f);
        
        GrabbedNewspaper?.Invoke();
    }
}
