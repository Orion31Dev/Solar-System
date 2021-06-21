using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CelestialBody), typeof(PlayerMovement))]
public class StatisticHandler : MonoBehaviour
{
    CelestialBody body;
    PlayerMovement movement;

    public CelestialBody sun, timberHearth;

    void Awake()
    {
        body = GetComponent<CelestialBody>();
        movement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = body.V + movement.V;
       // var speedSun = (v - sun.V).magnitude * Vector3.Dot(v, sun.transform.position);
        var speedSun =  Vector3.Dot(v, sun.V);
        var speedTimberHearth = Vector3.Distance(transform.position, timberHearth.transform.position) / (v - timberHearth.V).magnitude;
        UserInterface.UpdateUI(movement.V, speedSun, speedTimberHearth);
    }
}
