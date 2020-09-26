using UnityEngine;

public class Boid : MonoBehaviour
{
    Vector3 position = transform.position;
    Vector3 velocity = Vector3.zero;
    Vector3 acceleration = Vector3.zero;

    // TODO: Play with these values.
    public float maxSpeed = 10;
    public float maxSteeringForce = 2;

    void UpdateBoid()
    {
        // Acceleration is cleared every frame, because we don't want to add up forces from previous frames.
        acceleration = Vector3.zero;

        velocity += acceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        position += velocity * Time.deltaTime;
    }

    void SteerTowards(Vector3 target)
    {
        Vector3 desiredVelocity = (target - position).normalized * maxSpeed;
        Vector3 steeringForce = desiredVelocity - velocity;
        AddForce(Vector3.ClampMagnitude(steeringForce, maxSteeringForce));
    }

    void AddForce(Vector3 force)
    {
        // Acceleration = Sum of all forces / Mass.
        acceleration += force;
    }
}