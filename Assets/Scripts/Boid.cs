using UnityEngine;

public class Boid : MonoBehaviour
{
    Vector3 velocity = Vector3.zero;
    Vector3 acceleration = Vector3.zero;

    int numberOfNeighbors = 0;
    Vector3 avgFlockAlignment = Vector3.zero;
    Vector3 avgFlockPosition = Vector3.zero;
    Vector3 avgSeparationDirection = Vector3.zero;

    public float maxSpeed = 10.0f;
    public float maxSteeringForce = 2.0f;

    public float perceptionRadius = 2.0f;
    public float separationRadius = 1.0f;

    public float separationWeight = 1.0f;
    public float cohesionWeight = 1.0f;
    public float alignmentWeight = 1.0f;
    
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

    public void ApplyFlockingBehaviors(Boid[] boids) 
    {
        foreach (Boid other in boids)
        {
            Vector3 thisToOther = other.transform.position - transform.position;
            float squaredDist = thisToOther.x * thisToOther.x + thisToOther.y * thisToOther.y + thisToOther.z * thisToOther.z;
            if ((squaredDist < perceptionRadius * perceptionRadius) && (squaredDist > 0.001f))
            {
                numberOfNeighbors++;
                avgFlockAlignment += other.transform.forward;
                avgFlockPosition += other.transform.position;

                if (squaredDist < separationRadius * separationRadius)
                {
                    avgSeparationDirection += (-thisToOther)/(squaredDist); 
                }
            }
        }

        if (numberOfNeighbors >= 1) 
        {
            // Separation
            avgSeparationDirection /= numberOfNeighbors;
            acceleration += Steer(avgSeparationDirection) * separationWeight;

            // Cohesion
            avgFlockPosition /= numberOfNeighbors;
            acceleration += Steer(avgFlockPosition - transform.position) * cohesionWeight;

            // Alignment
            avgFlockAlignment /= numberOfNeighbors;
            acceleration += Steer(avgFlockAlignment) * alignmentWeight;

            // Some wandering force
            acceleration += new Vector3(Random.Range(0.25f, 1f), Random.Range(0.25f, 1f), Random.Range(0.25f, 1f));
        }
    }

    // Reynold's formula: Steering Force = Desired Velocity - Velocity
    Vector3 Steer(Vector3 AtoB)
    {
        Vector3 steeringForce = (AtoB.normalized * maxSpeed) - velocity;
        return Vector3.ClampMagnitude(steeringForce, maxSteeringForce);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////

    // (For debugging purposes)
    // Draw a transparent sphere for each property's radius.
    // void OnDrawGizmosSelected() 
    // {
    //     Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
    //     Gizmos.DrawSphere(transform.position, separationRadius);
    //     Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
    //     Gizmos.DrawSphere(transform.position, cohesionRadius);
    //     Gizmos.color = new Color(0f, 0f, 1f, 0.2f);
    //     Gizmos.DrawSphere(transform.position, alignmentRadius);
    // }
}