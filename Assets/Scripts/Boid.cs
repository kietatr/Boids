using UnityEngine;

public class Boid : MonoBehaviour
{
    Vector3 velocity = Vector3.zero;
    Vector3 acceleration = Vector3.zero;

    // TODO: Play with these values.
    public float maxSpeed = 10f;
    public float maxSteeringForce = 1f;
    public float separationRadius = 0.4f;
    public float cohesionRadius = 0.8f;

    GameObject sharedTarget;

    void Awake()
    {
        velocity = velocity.normalized * maxSpeed;

        sharedTarget = GameObject.FindWithTag("Target");
    }

    public void UpdateBoid()
    {
        // Acceleration is cleared every frame, because we don't want to add up forces from previous frames.
        acceleration = Vector3.zero;

        if (sharedTarget != null)
        {
            SteerTowards(sharedTarget.transform.position);
        }

        velocity += acceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        // Update the instantiated prefab.
        transform.position += velocity * Time.deltaTime;
        transform.forward = velocity.normalized;
    }

    // Push away from neighbors if they're too close.
    public void Separation(Boid[] boids)
    {
        Vector3 sumOfSeparationDirections = Vector3.zero;
        int numberOfSeparationDirections = 0;
        Vector3 averageSeparationDirection = Vector3.zero;

        foreach (Boid otherBoid in boids)
        {
            float distanceToOtherBoid = Vector3.Distance(transform.position, otherBoid.transform.position);

            // Make sure that the distance is not a distance to ourselveself, and that the distance is close enough.
            if ((distanceToOtherBoid > 0) && (distanceToOtherBoid < separationRadius))
            {
                // The direction to separate from a neighbor.
                Vector3 separationDirection = (transform.position - otherBoid.transform.position).normalized;

                // Add up all the separation directions from our neighbors, to find an average separation direction.
                sumOfSeparationDirections += separationDirection;
                numberOfSeparationDirections++;
            }

            if (numberOfSeparationDirections > 0)
            {
                averageSeparationDirection = sumOfSeparationDirections / numberOfSeparationDirections;
                SteerInDirectionOf(averageSeparationDirection);
            }
        }
    }

    // Stick to the average positions of our neighbors.
    public void Cohesion(Boid[] boids)
    {
        Vector3 sumOfNeighborsPositions = Vector3.zero;
        int numberOfNeighbors = 0;
        Vector3 averageNeighborsPosition = Vector3.zero;

        foreach (Boid otherBoid in boids)
        {
            float distanceToOtherBoid = Vector3.Distance(transform.position, otherBoid.transform.position);

            // Make sure that the distance is not a distance to ourselves, and that the distance is close enough.
            if ((distanceToOtherBoid > 0) && (distanceToOtherBoid < cohesionRadius))
            {
                // Add up all the positions of our neighbors.
                sumOfNeighborsPositions += otherBoid.transform.position;
                numberOfNeighbors++;
            }

            if (numberOfNeighbors > 0)
            {
                averageNeighborsPosition = sumOfNeighborsPositions / numberOfNeighbors;
                SteerTowards(averageNeighborsPosition);
            }
        }
    }

    void SteerInDirectionOf(Vector3 desiredDirection)
    {
        Vector3 desiredVelocity = desiredDirection * maxSpeed;
        Vector3 steeringForce = desiredVelocity - velocity;
        AddForce(Vector3.ClampMagnitude(steeringForce, maxSteeringForce));
    }

    void SteerTowards(Vector3 targetPosition)
    {
        Vector3 desiredVelocity = (targetPosition - transform.position).normalized * maxSpeed;
        Vector3 steeringForce = desiredVelocity - velocity;
        AddForce(Vector3.ClampMagnitude(steeringForce, maxSteeringForce));
    }

    void AddForce(Vector3 force)
    {
        // Acceleration = Sum of all forces / Mass.
        acceleration += force;
    }

    void OnDrawGizmosSelected() 
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
        Gizmos.DrawSphere(transform.position, cohesionRadius);
        Gizmos.color = new Color(1f, 0f, 1f, 0.2f);
        Gizmos.DrawSphere(transform.position, separationRadius);
    }
}