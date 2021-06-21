using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10;
    public float accelSpeed = 1;
    public float lookX = 30;
    public float lookY = 30;

    PlayerControls controls;

    Camera cam;

    public Vector3 V { get; private set; }
    Vector3 a;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Look.Mouse.performed += Look;

        cam = GetComponentInChildren<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void Update()
    {
        a = Vector3.zero;

        var wasd2d = controls.Move.WASD.ReadValue<Vector2>();
        var up = controls.Move.Up.ReadValue<float>();

        var wasd = new Vector3(wasd2d.x, up, wasd2d.y).normalized;

        a += transform.TransformDirection(wasd);

        V += a * Time.deltaTime * accelSpeed;
        V = Vector3.ClampMagnitude(V, moveSpeed);

        transform.position += V * Time.deltaTime;
    }

    private void Look(InputAction.CallbackContext ctx)
    {
        Vector3 rot = transform.rotation.eulerAngles;
        var delta = ctx.ReadValue<Vector2>();

        rot.y += delta.x * lookX * Time.deltaTime;
        rot.x -= delta.y * lookY * Time.deltaTime;

        transform.rotation = Quaternion.Euler(rot);
    }
}
