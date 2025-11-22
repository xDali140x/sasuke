using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerInput playerInput;
    private float force= 10f;
    private Rigidbody rb;
    private Vector2 input;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    private void Update()
    {
        input = playerInput.actions["Move"].ReadValue<Vector2>();
        Debug.Log(input);
        
    }
    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(input.x, 0, input.y) * force);
    }
}
