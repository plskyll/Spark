using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private float movingSpeed = 10f;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private GameObject deathEffectPrefab;

    private Vector2 inputVector;
    private Rigidbody2D rb;
    private int currentHealth;

    private float minMovingSpeed = 0.1f;
    private bool isRunning = false;
    private bool isDead = false;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    public void Start()
    {
        if (GameInput.Instance != null)
        {
            GameInput.Instance.OnPlayerAttack += GameInput_OnPlayerAttack;
        }
    }

    private void GameInput_OnPlayerAttack(object sender, EventArgs e)
    {
        if (!isDead)
        {
            ActiveWeapon.Instance.GetActiveWeapon().Attack();
        }
    }

    private void Update()
    {
        if (isDead) return;
        inputVector = GameInput.Instance.GetMovementVector();
    }

    private void FixedUpdate()
    {
        if (isDead) return;
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

    public void TakeDamage(Transform attacker, int damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        Debug.Log($"HP: {currentHealth}");
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        foreach (var sprite in GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.enabled = false;
        }
        
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;
    }
}