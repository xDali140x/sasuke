using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    public float maxSpeed = 1.8f;       // ajusta a gusto
    public float acceleration = 8f;     // cuánto tarda en llegar a la velocidad
    public float yawSmoothing = 0.08f;  // 0.05–0.12 suele ir bien
    public float yawDeadzone = 1.5f;    // grados para ignorar micro-vibración

    Rigidbody rb;
    PlayerInput pi;
    Transform head;                     // tu Main Camera
    Vector2 moveInput;
    float yawVel;                       // usado por SmoothDampAngle

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pi = GetComponent<PlayerInput>();
        head = Camera.main.transform;
    }

    void Update()
    {
        moveInput = pi.actions["Move"].ReadValue<Vector2>();

        // --- ALINEAR EL PLAYER AL YAW DE LA CABEZA, SUAVIZADO ---
        float targetYaw = head.eulerAngles.y;
        float currentYaw = transform.eulerAngles.y;
        float delta = Mathf.DeltaAngle(currentYaw, targetYaw);

        if (Mathf.Abs(delta) > yawDeadzone) {
            float smoothedYaw = Mathf.SmoothDampAngle(currentYaw, targetYaw, ref yawVel, yawSmoothing);
            transform.rotation = Quaternion.Euler(0f, smoothedYaw, 0f);
        }
    }

    void FixedUpdate()
    {
        // Dirección plana desde la cabeza (sin inclinar en pendientes)
        Vector3 fwd  = Vector3.ProjectOnPlane(head.forward, Vector3.up).normalized;
        Vector3 right = Vector3.Cross(Vector3.up, fwd);

        Vector3 desired = (fwd * moveInput.y + right * moveInput.x).normalized * maxSpeed;

        // Acelera/suaviza velocidad (no AddForce → menos “hielo”)
        Vector3 vel = rb.linearVelocity;
        Vector3 horiz = new Vector3(vel.x, 0, vel.z);
        Vector3 newHoriz = Vector3.MoveTowards(horiz, desired, acceleration * Time.fixedDeltaTime);
        rb.linearVelocity = new Vector3(newHoriz.x, vel.y, newHoriz.z);
    }
}
