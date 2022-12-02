using PaperBoy.LevelScrolling;
using Unity.Mathematics;
using UnityEngine;

public class BikeController : MonoBehaviour
{
    public GameObject frame;
    public GameObject handleBar;
    public GameObject frontWheel;
    public GameObject backWheel;
    public GameObject pedalGear;
    public GameObject leftPedal;
    public GameObject leftPedalEmpty;
    public GameObject rightPedal;
    public GameObject rightPedalEmpty;
    public GameObject seatStool;

    public float handleBarRotation;
    public Treadmill treadmill;
    public float wheelDiameter;
    public float pedalGearSpeed;

    void Update()
    {
        UpdatePedalTransforms();
        MovePedals();
        MoveWheels();
    }
    private void UpdatePedalTransforms()
    {
        leftPedal.transform.position = leftPedalEmpty.transform.position;
        rightPedal.transform.position = rightPedalEmpty.transform.position;
    }
    private float CalculateWheelRotationSpeed()
    {
        var treadmillSpeed = treadmill.GetTreadmillSpeed();
        return (treadmillSpeed/(wheelDiameter * math.PI)) * 360f;
    }

    private float CalculatePedalGearSpeed()
    {
        var treadmillSpeed = treadmill.GetTreadmillSpeed();
        return ((treadmillSpeed/(wheelDiameter * math.PI)) * 360f) * pedalGearSpeed;
    }

    private void MovePedals()
    {
        var pedalSpeed = CalculatePedalGearSpeed();
        pedalGear.transform.Rotate(Vector3.right * (pedalSpeed * Time.deltaTime));
    }

    private void MoveWheels()
    {
        var deltaTime = Time.deltaTime;
        var frontWheelTransform = frontWheel.transform;
        var backWheelTransform = backWheel.transform;
        var wheelSpeed = CalculateWheelRotationSpeed();

        Vector3 rotation = Vector3.right * (wheelSpeed * deltaTime);
        
        frontWheelTransform.Rotate(rotation);
        backWheelTransform.Rotate(rotation);
    }
}
