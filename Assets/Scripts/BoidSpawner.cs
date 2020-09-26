using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
    public GameObject boidPrefab;
    public int numberOfBoids = 10;
    public float spawnRadius = 2;
    // Boid[] boids;

    void Awake()
    {
        // boids = new Boid[numberOfBoids];

        for (int i = 0; i < numberOfBoids; i++)
        {
            GameObject boid = Instantiate(boidPrefab);
            boid.transform.position = transform.position + Random.insideUnitSphere * spawnRadius;
            boid.transform.forward = Random.onUnitSphere;

            // boids[i] = boid;
        }
    }

    // Draw a transparent sphere in the editor to visualize the spawn radius.
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 0.2f, 0.4f, 0.2f);
        Gizmos.DrawSphere(Vector3.zero, spawnRadius);
    }
}
