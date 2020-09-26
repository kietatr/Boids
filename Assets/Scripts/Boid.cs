using UnityEngine;

public class Boid : MonoBehaviour
{
    Vector3 velocity = Vector3.zero;
    Vector3 acceleration = Vector3.zero;

    // TODO: Play with these values.
    public float maxSpeed = 10;
    public float maxSteeringForce = 0.1f;

    GameObject target;

    void Awake()
    {
        velocity = velocity.normalized * maxSpeed;

        target = GameObject.FindWithTag("Target");
    }

    public void UpdateBoid()
    {
        // Acceleration is cleared every frame, because we don't want to add up forces from previous frames.
        acceleration = Vector3.zero;

        if (target != null) {
            SteerTowards(target.transform.position);
        }

        velocity += acceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        // Update position
        transform.position += velocity * Time.deltaTime;
        transform.forward = velocity;
    }

    void SteerTowards(Vector3 target)
    {
        Vector3 desiredVelocity = (target - transform.position).normalized * maxSpeed;
        Vector3 steeringForce = desiredVelocity - velocity;
        AddForce(Vector3.ClampMagnitude(steeringForce, maxSteeringForce));
    }

    void AddForce(Vector3 force)
    {
        // Acceleration = Sum of all forces / Mass.
        acceleration += force;
    }
}