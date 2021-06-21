using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class CelestialBody : MonoBehaviour
{
    public float Mass { get; private set; }

    public float surfaceGravity;
    public float radius;
    public Vector3 initialVelocity;

    new public Rigidbody rigidbody;

    public Vector3 V { get; private set; }

    public Color color;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();

        V = initialVelocity;
        CalculateMassAndRadius();
    }

    private void OnValidate()
    {
        CalculateMassAndRadius();
    }

    private void CalculateMassAndRadius()
    {
        Mass = surfaceGravity * radius * radius / Universe.G;

        if (TryGetComponent<Planet>(out var planet)) radius = planet.shapeSettings.planetRadius;
    }

    public void UpdateVelocity(CelestialBody[] bodies, float timeStep)
    {
        foreach (var other in bodies)
        {
            if (other == this) continue;

            float sqrDist = (other.rigidbody.position - rigidbody.position).sqrMagnitude;
            Vector3 forceDir = (other.rigidbody.position - rigidbody.position).normalized;
            Vector3 force = forceDir * Universe.G * other.Mass / sqrDist;
            Vector3 accel = force;

            V += accel * timeStep * 1;
        }
    }

    public void UpdatePosition(float timeStep)
    {
        rigidbody.position += V * timeStep;
    }
}
