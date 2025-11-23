using UnityEngine;
using UnityEngine.InputSystem;

public class VRLocomotion : MonoBehaviour
{
    [Header("References")]
    public Transform head;                     // Main Camera
    public Rigidbody rb;                       // Rigidbody del Player
    public InputActionReference moveAction;    // Player/Move (Vector2)

    [Header("Movement")]
    [Range(0.5f, 5f)] public float moveSpeed = 1.8f;
    [Range(0f, 0.3f)] public float deadzone = 0.08f; // evita micro drift del stick
    [Range(0f, 20f)] public float accel = 12f;       // suavizado de cambio de velocidad

    Vector3 _vel; // velocidad objetivo suavizada

    void Reset()
    {
        rb = GetComponent<Rigidbody>();
        if (Camera.main) head = Camera.main.transform;
    }

    void OnEnable()  => moveAction?.action?.Enable();
    void OnDisable() => moveAction?.action?.Disable();

    void FixedUpdate()
    {
        if (rb == null || head == null || moveAction == null) return;

        // 1) Leemos el stick
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        if (input.magnitude < deadzone) input = Vector2.zero;

        // 2) Solo usamos el Yaw de la cabeza (sin pitch/roll) para orientar el movimiento
        float yaw = head.eulerAngles.y;
        Quaternion yawRot = Quaternion.Euler(0f, yaw, 0f);

        // 3) Dirección objetivo (en el plano XZ)
        Vector3 wishDir = yawRot * new Vector3(input.x, 0f, input.y);
        Vector3 targetVel = wishDir.normalized * moveSpeed;

        // 4) Suavizado de velocidad para evitar “sacudidas”
        _vel = Vector3.MoveTowards(_vel, targetVel, accel * Time.fixedDeltaTime);

        // 5) Avance estable con físicas
        Vector3 nextPos = rb.position + _vel * Time.fixedDeltaTime;
        rb.MovePosition(nextPos);
        // (No rotamos el Player aquí; la vista la da la cabeza. Si quieres, puedes
        // orientar el Player al yaw para “alinear” el collider:)
        // rb.MoveRotation(Quaternion.Euler(0f, yaw, 0f));
    }
}
