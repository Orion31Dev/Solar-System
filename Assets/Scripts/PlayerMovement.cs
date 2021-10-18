using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10;
    public float accelSpeed = 1;
    public float lookX = 30f;
    public float lookY = 30f;

    PlayerControls controls;

    Camera cam;
    new Rigidbody rigidbody;

    public Vector3 V { get; set; }
    Vector3 a;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Look.Mouse.performed += Look;

        cam = GetComponentInChildren<Camera>();
        rigidbody = GetComponentInChildren<Rigidbody>();

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
        
        rigidbody.velocity += a * Time.deltaTime * accelSpeed;
    }

    private void Look(InputAction.CallbackContext ctx)
    {
        Vector3 rot = transform.rotation.eulerAngles;
        var delta = ctx.ReadValue<Vector2>();

        rot.y += delta.x * lookX;
        rot.x -= delta.y * lookY;

        transform.rotation = Quaternion.Euler(rot);
    }
}
