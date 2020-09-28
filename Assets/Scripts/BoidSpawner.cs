using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
    public Boid boidPrefab;
    public int numberOfBoids = 100;
    public float spawnRadius = 2;
    public bool stayInsideSphere = true;

    Boid[] boids;

    void Awake()
    {
        boids = new Boid[numberOfBoids];

        for (int i = 0; i < numberOfBoids; i++)
        {
            Boid boid = Instantiate(boidPrefab, transform);
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
            boid.ApplyFlockingBehaviors(boids);
            if (stayInsideSphere)
            {
                boid.StayInsideSphereBoundaries(transform.position, spawnRadius);
            }
        }
    }

    // (For debugging purposes)
    // Draw a transparent sphere in the Unity editor to visualize the spawn radius.
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 0.2f, 0.4f, 0.2f);
        Gizmos.DrawSphere(transform.position, spawnRadius);
    }
}
