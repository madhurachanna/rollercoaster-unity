using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

public class SplineFollow : MonoBehaviour
{
    [SerializeField] private SplineContainer track;
    [SerializeField] private float directionSwitchThreshold = 0.3f;
    [SerializeField] private float velocityDampeningThreshold = 2.5f;
    [SerializeField] private float velocityDampeningRate = 0.8f;
    [SerializeField] private float stopMinAngleThreshold = 12f;
    [SerializeField] public bool isFollowingSpline = true;

    private Rigidbody rb;
    private GameObject car;
    private Vector3 velocity;
    private float velocityMagnitude;
    private float direction;
    private float angle;
    private bool movingBackward;

    private int resolution = 4;
    private int iterations = 2;

    private void Start()
    {
        car = gameObject;
        rb = car.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isFollowingSpline)
            UpdatePosition();
    }

    private void FixedUpdate()
    {
        if (isFollowingSpline)
            UpdatePhysics();
    }

    private void UpdatePosition()
    {
        var native = new NativeSpline(track.Spline);
        float distance = SplineUtility.GetNearestPoint(native, car.transform.position, out float3 nearest, out float t, resolution, iterations);

        car.transform.position = nearest;

        Vector3 forward = Vector3.Normalize(track.EvaluateTangent(t));
        Vector3 up = track.EvaluateUpVector(t);

        var remappedForward = new Vector3(0, 0, 1);
        var remappedUp = new Vector3(0, 1, 0);
        var axisRemapRotation = Quaternion.Inverse(Quaternion.LookRotation(remappedForward, remappedUp));

        car.transform.rotation = Quaternion.LookRotation(forward, up) * axisRemapRotation;
    }

    private void UpdatePhysics()
    {
        Vector3 engineForward = car.transform.forward;
        direction = Vector3.Dot(rb.velocity, engineForward);

        UpdateMovingBackwardState();

        if (movingBackward)
            engineForward *= -1;

        UpdateAngle();

        if (rb.velocity.magnitude < velocityDampeningThreshold && angle < stopMinAngleThreshold)
            ApplyVelocityDamping(engineForward);
        else
            rb.velocity = rb.velocity.magnitude * engineForward;

        velocity = rb.velocity;
        velocityMagnitude = rb.velocity.magnitude;
    }

    private void UpdateMovingBackwardState()
    {
        if (movingBackward)
        {
            if (direction > -directionSwitchThreshold)
                movingBackward = false;
        }
        else
        {
            if (direction < directionSwitchThreshold)
                movingBackward = true;
        }
    }

    private void UpdateAngle()
    {
        angle = Vector3.Angle(car.transform.up, Vector3.up);
    }

    private void ApplyVelocityDamping(Vector3 engineForward)
    {
        rb.velocity = rb.velocity.magnitude * engineForward;
        rb.velocity *= velocityDampeningRate;
    }
}