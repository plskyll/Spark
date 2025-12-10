using System.Collections;
using System;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(EnemyAI))]
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyEntity : MonoBehaviour
{
    [SerializeField] private EnemySO enemySO;

    public event EventHandler OnTakeHit;
    public event EventHandler OnDeath;
    
    private int currentHealth;
    private float damageCooldown = 1.0f;
    private float lastDamageTime;

    private CapsuleCollider2D capsuleCollider2D;
    private BoxCollider2D boxCollider2D;
    private EnemyAI enemyAI;
    private SpriteRenderer[] spriteRenderers;
    private Material[] materials;
    private int dissolveAmountID;

    private void Awake()
    {
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        enemyAI = GetComponent<EnemyAI>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        materials = new Material[spriteRenderers.Length];
        dissolveAmountID = Shader.PropertyToID("_DissolveAmount");
        
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            materials[i] = spriteRenderers[i].material;
            if (materials[i].HasProperty(dissolveAmountID))
            {
                materials[i].SetFloat(dissolveAmountID, 0f);
            }
        }

        if (enemySO == null)
        {
            currentHealth = 100;
        }
        else
        {
            currentHealth = enemySO.enemyHealth;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        TryDamagePlayer(collision.gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        TryDamagePlayer(collision.gameObject);
    }

    private void TryDamagePlayer(GameObject target)
    {
        if (target.TryGetComponent(out Player player))
        {
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                if (enemySO != null)
                {
                    player.TakeDamage(transform, enemySO.enemyDamageAmount);
                    lastDamageTime = Time.time;
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        OnTakeHit?.Invoke(this, EventArgs.Empty);
        DetectDeath();
    }

    public void CapsuleColliderTurnOff()
    {
        capsuleCollider2D.enabled = false;
    }

    public void CapsuleColliderTurnOn()
    {
        capsuleCollider2D.enabled = true;
    }

    private void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            boxCollider2D.enabled = false;
            capsuleCollider2D.enabled = false;
            
            if (enemyAI != null) enemyAI.SetDeathState();

            OnDeath?.Invoke(this, EventArgs.Empty);
            
            StartCoroutine(DissolveRoutine());
        }
    }

    private IEnumerator DissolveRoutine()
    {
        float duration = 1.0f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float lerpAmount = elapsedTime / duration;
            
            foreach (var mat in materials)
            {
                if (mat.HasProperty(dissolveAmountID))
                {
                    mat.SetFloat(dissolveAmountID, lerpAmount);
                }
                else
                {
                    Color color = mat.color;
                    color.a = 1f - lerpAmount;
                    mat.color = color;
                }
            }
            
            yield return null;
        }

        foreach (var mat in materials)
        {
            if (mat.HasProperty(dissolveAmountID))
            {
                mat.SetFloat(dissolveAmountID, 1f);
            }
        }
        
        Destroy(gameObject);
    }
}