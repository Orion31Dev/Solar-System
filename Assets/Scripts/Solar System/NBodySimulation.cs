using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NBodySimulation : MonoBehaviour
{
    CelestialBody[] bodies;

    private void Awake()
    {
        bodies = FindObjectsOfType<CelestialBody>();
        Time.fixedDeltaTime = Universe.physicsTimeStep;
    }

    private void FixedUpdate()
    {
        foreach (var body in bodies)
        {
            body.UpdateVelocity(bodies, Universe.physicsTimeStep);
        }

        foreach (var body in bodies)
        {
            body.UpdatePosition(Universe.physicsTimeStep);
        }
    }
}
