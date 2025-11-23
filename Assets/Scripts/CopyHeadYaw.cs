using UnityEngine;

public class CopyHeadYaw : MonoBehaviour
{
    [SerializeField] Transform head; // arrastra Main Camera aquí
    [SerializeField] float turnSpeed = 6f; // 4–8 va bien

    void LateUpdate()
    {
        if (!head) return;
        // Sólo copiamos Y (yaw) de la cabeza
        float targetYaw = head.eulerAngles.y;
        Vector3 e = transform.eulerAngles;
        e.y = Mathf.LerpAngle(e.y, targetYaw, Time.deltaTime * turnSpeed);
        transform.eulerAngles = e;
    }
}
