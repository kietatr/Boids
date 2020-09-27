using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
    public Boid boidPrefab;
    public int numberOfBoids = 10;
    public float spawnRadius = 2;
    Boid[] boids;

    void Awake()
    {
        boids = new Boid[numberOfBoids];

        for (int i = 0; i < numberOfBoids; i++)
        {
            Boid boid = Instantiate(boidPrefab);
            boid.transform.position = transform.position + Random.insideUnitSphere * spawnRadius;
            boid.transform.forward = Random.onUnitSphere;
            boids[i] = boid;
        }
    }

    void Update()
    {
        foreach (Boid boid in boids)
        {
            boid.UpdateBoid();
            boid.Separation(boids);
            boid.Cohesion(boids);
        }
    }

    // (For debugging purposes)
    // Draw a transparent sphere in the Unity editor to visualize the spawn radius.
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 0.2f, 0.4f, 0.2f);
        Gizmos.DrawSphere(Vector3.zero, spawnRadius);
    }
}
