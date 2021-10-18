using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(CelestialBody), typeof(PlayerMovement))]
public class UIHandler : MonoBehaviour
{
    CelestialBody playerBody;
    PlayerMovement movement;

    CelestialBody selected;

    PlayerControls controls;

    PlanetSelectionUI planetSelectionUI;

    public Camera cam;

    public Text text;

    public RectTransform canvas;

    [SerializeField]
    Color towards, away;

    [SerializeField]
    Texture arrowMesh, squareMesh;

    [SerializeField]
    RectTransform horizArrow, vertArrow;

    void Awake()
    {
        playerBody = GetComponent<CelestialBody>();
        movement = GetComponent<PlayerMovement>();

        controls = new PlayerControls();
        controls.Enable();
        controls.Interaction.Click.performed += SelectCelestialBody;

        planetSelectionUI = new PlanetSelectionUI(cam, squareMesh, arrowMesh, horizArrow, vertArrow);
    }

    private void Update()
    {
        if (selected == null) return;//|| Vector3.Dot(cam.transform.forward, selected.transform.position - cam.transform.position) > 0) return;

        float[] speeds = GetRelativeSpeedToBody(selected);
        planetSelectionUI.Update(selected, playerBody, speeds[2], speeds[0], speeds[1]);
    }

    // Update is called once per frame
    void OnGUI()
    {
        if (selected == null || !CameraCanSeePoint(cam, selected.transform.position)) return;//|| Vector3.Dot(cam.transform.forward, selected.transform.position - cam.transform.position) > 0) return;

        //GUI.Label(new Rect(100, 100, 100, 100), $"velocity: {playerBody.rigidbody.velocity}");
        planetSelectionUI.DrawGUI();
        //if (Keyboard.current.spaceKey.wasPressedThisFrame) movement.V = selected.V - self.V;

        /*
        Vector3 p1 = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.transform.position.z));
        Vector3 p2 = cam.ScreenToWorldPoint(new Vector3(1, 0, cam.transform.position.z));
        float unit = Vector3.Distance(p1, p2);

        float screenRadius = selected.radius / unit;

        var speeds = GetRelativeSpeedToBody(selected);
        var viewPoint = cam.WorldToViewportPoint(selected.transform.position +
            new Vector3(selected.radius - (Mathf.Sign(cam.transform.forward.z) > 0 ? screenRadius / Screen.width : 10 * screenRadius / Screen.width) * Vector3.Distance(playerBody.transform.position, selected.transform.position), 0, 0));

        if (Mathf.Abs(speeds[2]) < 1) text.color = Color.white;
        else if (speeds[2] > 0) text.color = towards;
        else text.color = away;

        text.text = $"{selected.name}\n{Vector3.Distance(selected.transform.position, playerBody.transform.position)} km \n{speeds[2]:N1} m/s";
        textTransform.anchorMin = textTransform.anchorMax = viewPoint + new Vector3(0, 0.07f, 0);


        Vector2 screenPoint = cam.WorldToScreenPoint(selected.transform.position);
        screenPoint.y = Screen.height - screenPoint.y;


        // Arrows
        var vx = Mathf.Abs(speeds[0]);
        var vy = Mathf.Abs(speeds[1]);

        for (int i = 0; i < Mathf.Floor(vx); i++)
        {
            var arrow = i == Mathf.Floor(vx) - 1;

            Rect rect = new Rect(screenPoint + (speeds[0] >= 0 ? 1 : -1) * new Vector2(50 * i + screenRadius, 0), Vector2.one * (arrow ? 20 : 10));

            if (arrow) GUI.DrawTexture(rect, arrowMesh);
            else GUI.DrawTexture(rect, squareMesh);
        }

        for (int i = 0; i < Mathf.Floor(vy); i++)
        {
            var arrow = i == Mathf.Floor(vy) - 1;

            Rect rect = new Rect(screenPoint + (speeds[1] <= 0 ? 1 : -1) * new Vector2(0, 50 * i + screenRadius), Vector2.one * (arrow ? 20 : 10));

            if (arrow) GUI.DrawTexture(rect, arrowMesh);
            else GUI.DrawTexture(rect, squareMesh);
        }
        */
    }

    bool CameraCanSeePoint(Camera cam, Vector3 point)
    {
        var vp = cam.WorldToViewportPoint(point);
        return !(vp.x > 1 || vp.x < 0 || vp.y > 1 || vp.y < 0 || vp.z < 0);
    }

    void SelectCelestialBody(InputAction.CallbackContext ctx)
    {
        var ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out var hit))
        {
            if (hit.transform.TryGetComponent<CelestialBody>(out var body) && selected != body)
            {
                selected = body;
            }
            else selected = null;
        }
    }

    float[] GetRelativeSpeedToBody(CelestialBody body)
    {
        Vector3 dirToPlanet = (body.transform.position - cam.transform.position).normalized;
        //float dstToPlanetCentre = (body.transform.position - cam.transform.position).magnitude;

        Vector3 horizontal = Vector3.Cross(dirToPlanet, cam.transform.up).normalized;
        horizontal *= Mathf.Sign(Vector3.Dot(horizontal, cam.transform.right)); // make sure roughly same direction as right vector of cam
        Vector3 vertical = Vector3.Cross(dirToPlanet, horizontal).normalized;
        vertical *= Mathf.Sign(Vector3.Dot(vertical, cam.transform.up));

        // Calculate relative velocity
        Vector3 relativeVelocityWorldSpace = playerBody.rigidbody.velocity - body.V;

        float vx = -Vector3.Dot(relativeVelocityWorldSpace, horizontal);
        float vy = -Vector3.Dot(relativeVelocityWorldSpace, vertical);
        float vz = Vector3.Dot(relativeVelocityWorldSpace, dirToPlanet);
        return new float[] { vx, vy, vz };

        /*
        var vD = body.transform.position - transform.position;
        var v = movement.V;

        return Vector3.Dot(vD, v) / vD.magnitude;*/
    }
}
