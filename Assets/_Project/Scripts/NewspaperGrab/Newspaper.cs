using Core.Events;
using Core.Sets;
using Core.UpdateManager;
using PaperBoy.LevelScrolling;
using PaperBoy.ObjectPool;
using PaperBoy.Procedural;
using UnityEngine;

public class Newspaper : PoolableObject, IUpdateable
{
    [SerializeField] private UpdateRuntimeSet lateUpdateRuntimeSet;
    [SerializeField] private TreadmillSO treadmillSO;
    [SerializeField] private Rigidbody thisRigidbody;
    [SerializeField] private RuntimeGameEventListener throwEvent;
    [SerializeField] private RuntimeGameEventListener hitEvent;

    private bool _wasThrown;
    private bool _hasRegisteredHit;
    private float _speed;

    private const float SPIN_MULTIPLIER = 2.5f;
    private const float MAX_ANGULAR_VELOCITY = 50;
    private const float ANGULAR_VELOCITY_DAMPENER = .5f;
    private const float RAYCAST_DISTANCE = .1f;


    protected override void OnValidate()
    {
        base.OnValidate();
        
        thisRigidbody = GetComponent<Rigidbody>();
    }

    protected override void Awake()
    {
        base.Awake();
        
        thisRigidbody.maxAngularVelocity = MAX_ANGULAR_VELOCITY;
    }

    private void OnEnable()
    {
        _hasRegisteredHit = false;
        _wasThrown = false;
        
        thisRigidbody.isKinematic = true;
    }

    private void OnDisable()
    {
        thisRigidbody.velocity = Vector3.zero;
        thisRigidbody.angularVelocity = Vector3.zero;
        
        lateUpdateRuntimeSet.Remove(this);
    }
    
    public bool IsValid()
    {
        return isActiveAndEnabled;
    }

    public void ManagedUpdate() { }
    public void ManagedFixedUpdate() { }

    public void ManagedLateUpdate()
    {
        Move(Vector3.back * (treadmillSO.MovementSpeed * Time.deltaTime));
    }
    public void SmartUpdate() { }
    
    public void Move(Vector3 movement)
    {
        ThisTransform.Translate(movement, Space.World);
    }

    public void Throw(Vector3 velocity)
    {
        _wasThrown = true;
        
        ThisTransform.parent = null;

        velocity = velocity.magnitude > 0 ? velocity : new Vector3(1, 1, 1);
        
        thisRigidbody.isKinematic = false;
        thisRigidbody.velocity = velocity;
        thisRigidbody.angularVelocity = Vector3.zero;

        _speed = velocity.magnitude;
        thisRigidbody.AddTorque(transform.right * (_speed * SPIN_MULTIPLIER), ForceMode.VelocityChange);
        
        thisRigidbody.useGravity = true;
        
        throwEvent.OnEventRaised();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!_wasThrown)
        {
            return;
        }

        AttemptRegisterHit();
        
        if (_wasThrown)
        {
            _wasThrown = false;
        }
    }

    private void AttemptRegisterHit()
    {
        if (_hasRegisteredHit)
        {
            return;
        }
        
        _hasRegisteredHit = true;
        thisRigidbody.angularVelocity *= ANGULAR_VELOCITY_DAMPENER;
        
        lateUpdateRuntimeSet.Add(this);
        
        hitEvent.OnEventRaised();
    }
}
