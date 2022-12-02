using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class ObstacleLauncher : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private float force;

    public UnityEvent impactEvent;

    private void OnCollisionEnter(Collision collision)
    {
        var collisionTransform = collision.transform;
        if (collisionTransform.CompareTag("Player"))
        {
            Vector3 dir = collision.contacts[0].point - transform.position;
            dir = -dir.normalized;
            rigidbody.AddForce(dir * force, ForceMode.Impulse);

            var torque = new Vector3
            {
                x = Random.Range (-200, 200),
                y = Random.Range (-200, 200),
                z = Random.Range (-200, 200)
            };

            rigidbody.AddTorque(torque, ForceMode.Impulse);
            impactEvent?.Invoke();
        }
    }
}
