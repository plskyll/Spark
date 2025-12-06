using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance  { get; private set; }
    
    [SerializeField] private float movingSpeed = 10f;
    private Vector2 inputVector;
    private Rigidbody2D rb;
    
    private float minMovingSpeed = 0.1f;
    private bool isRunning = false;
    
    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    public void Start()
    {
        GameInput.Instance.OnPlayerAttack += GameInput_OnPlayerAttack;

    }

    private void GameInput_OnPlayerAttack(object sender, EventArgs e)
    {
        ActiveWeapon.Instance.GetActiveWeapon().Attack();
    }
    
    private void Update()
    {
        inputVector  = GameInput.Instance.GetMovementVector();
    }
    
    private void FixedUpdate()
    {
        HandleMovement();
    }
    
    private void HandleMovement()
    {
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
    
    public Vector3 GetPlayerScreenPosition()
    {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }
} 
