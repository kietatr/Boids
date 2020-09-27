using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidNonOptimized : MonoBehaviour
{
    Vector3 velocity = Vector3.zero;
    Vector3 acceleration = Vector3.zero;

    public float minSpeed = 3f;
    public float maxSpeed = 10f;
    public float maxSteeringForce = 2f;

    public float separationRadius = 1f;
    public float cohesionRadius = 2f;
    public float alignmentRadius = 2f;

    public float separationWeight = 1;
    public float cohesionWeight = 1;
    public float alignmentWeight = 1;

    GameObject sharedTarget;

    void Awake()
    {
        sharedTarget = GameObject.FindWithTag("Target");
    }

    void Start()
    {
        // velocity = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f));
        velocity = transform.forward * ((minSpeed + maxSpeed) / 2);
    }

    public void UpdateBoid()
    {
        velocity += acceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        // Update the instantiated prefab.
        transform.position += velocity * Time.deltaTime;
        transform.forward = velocity.normalized;

        // Acceleration is cleared every frame, because we don't want to add up forces from previous frames.
        acceleration = Vector3.zero;
    }

    // Reynold's formula: Steering Force = Desired Velocity - Velocity
    Vector3 SteerTowards(Vector3 targetPosition)
    {
        Vector3 desiredVelocity = (targetPosition - transform.position).normalized * maxSpeed;
        Vector3 steeringForce = desiredVelocity - velocity;
        return Vector3.ClampMagnitude(steeringForce, maxSteeringForce);
    }

    Vector3 SteerInDirectionOf(Vector3 desiredDirection)
    {
        Vector3 desiredVelocity = desiredDirection * maxSpeed;
        Vector3 steeringForce = desiredVelocity - velocity;
        return Vector3.ClampMagnitude(steeringForce, maxSteeringForce);
    }

    // Push away from neighbors if they're too close.
    Vector3 SeparationForce(BoidNonOptimized[] boids)
    {
        Vector3 separationForce = Vector3.zero;

        Vector3 sumOfSeparationDirections = Vector3.zero;
        int numberOfNeighbors = 0;
        Vector3 averageSeparationDirection = Vector3.zero;

        foreach (BoidNonOptimized otherBoid in boids)
        {
            float distanceToOtherBoid = Vector3.Distance(transform.position, otherBoid.transform.position);

            // Make sure that the distance is not a distance to myself, and that the distance is close enough.
            if ((distanceToOtherBoid > 0.01f) && (distanceToOtherBoid < separationRadius))
            {
                // Debug.DrawLine(transform.position, otherBoid.transform.position, new Color(1f, 0f, 0f, 0.2f));

                // The direction that points away from a neighbor.
                Vector3 separationDirection = (transform.position - otherBoid.transform.position).normalized;

                // The closer the distance, the stronger the separation. The farther, the weaker.
                // separationDirection /= distanceToOtherBoid;

                // Add up all the separation directions from my neighbors, to find an average separation direction.
                sumOfSeparationDirections += separationDirection;
                numberOfNeighbors++;
            }
        }

        if (numberOfNeighbors >= 1)
        {
            averageSeparationDirection = sumOfSeparationDirections / numberOfNeighbors;
            separationForce = SteerInDirectionOf(averageSeparationDirection.normalized);
        }

        return separationForce;
    }

    // Stick to the average positions of my neighbors.
    Vector3 CohesionForce(BoidNonOptimized[] boids)
    {
        Vector3 cohesionForce = Vector3.zero;

        Vector3 sumOfNeighborsPositions = Vector3.zero;
        int numberOfNeighbors = 0;
        Vector3 averageNeighborsPosition = Vector3.zero;

        foreach (BoidNonOptimized otherBoid in boids)
        {
            float distanceToOtherBoid = Vector3.Distance(transform.position, otherBoid.transform.position);

            // Make sure that the distance is not a distance to myself, and that the distance is close enough.
            if ((distanceToOtherBoid > 0) && (distanceToOtherBoid < cohesionRadius))
            {
                // Debug.DrawLine(transform.position, otherBoid.transform.position, new Color(0f, 1f, 0f, 0.2f));

                // Add up all the positions of my neighbors.
                sumOfNeighborsPositions += otherBoid.transform.position;
                numberOfNeighbors++;
            }
        }

        if (numberOfNeighbors >= 1)
        {
            averageNeighborsPosition = sumOfNeighborsPositions / numberOfNeighbors;
            cohesionForce = SteerTowards(averageNeighborsPosition);
        }

        return cohesionForce;
    }

    // Align with the average direction of my neighbors.
    void AlignmentForce(BoidNonOptimized[] boids)
    {
        Vector3 averageVelocity = Vector3.zero;
        int numberOfNeighbors = 0;

        foreach (BoidNonOptimized otherBoid in boids)
        {
            float sqrDistanceToOtherBoid = (transform.position - otherBoid.transform.position).sqrMagnitude;

            if ((sqrDistanceToOtherBoid > 0) && (sqrDistanceToOtherBoid < (alignmentRadius * alignmentRadius)))
            {
                averageVelocity += otherBoid.velocity;
                numberOfNeighbors++;
            }
        }

        if (numberOfNeighbors >= 1)
        {
            averageVelocity *= (1.0f / numberOfNeighbors);
            acceleration += (averageVelocity - velocity).normalized * alignmentWeight;
        }
    }

    public void ApplyFlockingBehaviors(BoidNonOptimized[] boids)
    {
        Vector3 separationForce = SeparationForce(boids);
        acceleration += separationForce * separationWeight;

        Vector3 cohesionForce = CohesionForce(boids);
        acceleration += cohesionForce * cohesionWeight;

        AlignmentForce(boids);

        // Vector3 seekTargetForce = Vector3.zero;

        // if (sharedTarget != null)
        // {
        //     seekTargetForce = SteerTowards(sharedTarget.transform.position);
        // }
        // acceleration += seekTargetForce;
    }
}
