using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class HeadRelativeMove : MonoBehaviour
{
    [Header("References")]
    public Transform head;            // arrastra aquí la Main Camera
    Rigidbody rb;
    PlayerInput pi;

    [Header("Tuning")]
    public string moveActionName = "Move";
    public float maxSpeed = 1.8f;     // m/s: camina suave
    public float acceleration = 8f;   // cómo de rápido alcanza maxSpeed
    public float turnToHeadSpeed = 360f; // °/s para alinear el cuerpo al yaw de la cabeza

    Vector2 moveInput;
    Vector3 velocity;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pi = GetComponent<PlayerInput>();

        // Ajustes de estabilidad recomendados:
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; // evita volcar
        rb.interpolation = RigidbodyInterpolation.Interpolate;         // visual suave entre pasos de física
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // mejor contacto con colisiones rápidas
        rb.angularDamping = 0.6f;
        rb.linearDamping = 1f;
    }

    void Update()
    {
        moveInput = pi.actions[moveActionName].ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        // Direcciones horizontales según la cabeza (ignorando inclinación)
        Vector3 fwd = Vector3.ProjectOnPlane(head.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(head.right, Vector3.up).normalized;

        // Deseo de movimiento
        Vector3 wishDir = fwd * moveInput.y + right * moveInput.x;
        if (wishDir.sqrMagnitude > 1f) wishDir.Normalize();

        // Suaviza la velocidad (aceleración/desaceleración controlada)
        Vector3 targetVel = wishDir * maxSpeed;
        velocity = Vector3.MoveTowards(velocity, targetVel, acceleration * Time.fixedDeltaTime);

        // Traslación estable por física
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);

        // Gira el cuerpo hacia el yaw de la cabeza (opcional pero da naturalidad)
        float headYaw = Mathf.Atan2(fwd.x, fwd.z) * Mathf.Rad2Deg;
        Quaternion targetRot = Quaternion.Euler(0f, headYaw, 0f);
        rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRot, turnToHeadSpeed * Time.fixedDeltaTime));
    }
}
