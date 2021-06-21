using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;

[RequireComponent(typeof(CelestialBody), typeof(PlayerMovement))]
public class StatisticHandler : MonoBehaviour
{
    CelestialBody body;
    PlayerMovement movement;

    CelestialBody selected;

    PlayerControls controls;

    public Camera cam;

    void Awake()
    {
        body = GetComponent<CelestialBody>();
        movement = GetComponent<PlayerMovement>();

        controls = new PlayerControls();
        controls.Enable();
        controls.Interaction.Click.performed += SelectCelestialBody;
    }

    // Update is called once per frame
    void Update()
    {
        if (selected == null) return;

        var speed = DistanceToBody(selected);
    }

    void SelectCelestialBody(InputAction.CallbackContext ctx)
    {
        Debug.Log("eee");
        var ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out var hit))
        {
            if (hit.transform.TryGetComponent<CelestialBody>(out var body))
            {
                this.selected = body;
            }
        }
    }

    float DistanceToBody(CelestialBody body)
    {
        var vD = body.transform.position - transform.position;
        var v = this.body.V + movement.V;

        return Vector3.Dot(vD, v) / vD.magnitude;
    }
}
