using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    float rotationVelocity;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * speed;
        
        if (moveInput.sqrMagnitude > 0.001f)
        {
            float targetAngle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;

            float smoothAngle = Mathf.SmoothDampAngle(
                transform.eulerAngles.z,
                targetAngle,
                ref rotationVelocity,
                0.08f
            );

            transform.rotation = Quaternion.Euler(0, 0, smoothAngle);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}
