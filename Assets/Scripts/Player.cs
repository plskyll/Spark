using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance  { get; private set; }
    
    [SerializeField] private float movingSpeed = 10f;    
    
    private Rigidbody2D rb;
    
    private float minMovingSpeed = 0.1f;
    private bool isRunning = false;
    
    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void FixedUpdate()
    {
        HandleMovement();
    }
    
    private void HandleMovement()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVector();
        inputVector.Normalize();
        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));
        
        if (Math.Abs(inputVector.x) > minMovingSpeed || Math.Abs(inputVector.y) > minMovingSpeed)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }
    
    public bool IsRunning()
    {
        return isRunning;
    }
} 
