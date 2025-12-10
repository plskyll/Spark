using System.Collections;
using System;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(EnemyAI))]
public class EnemyEntity : MonoBehaviour
{
    [SerializeField] private EnemySO enemySO;
    [SerializeField] private GameObject dissolveEffectPrefab; 

    public event EventHandler OnTakeHit;
    public event EventHandler OnDeath;
    
    private int currentHealth;

    private CapsuleCollider2D capsuleCollider2D;
    private BoxCollider2D boxCollider2D;
    private EnemyAI enemyAI;

    private void Awake()
    {
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        enemyAI = GetComponent<EnemyAI>();
    }

    private void Start()
    {
        if (enemySO == null)
        {
            Debug.LogError($"[EnemyEntity] Enemy SO not assigned on {gameObject.name}");
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
            if (enemySO != null)
            {
                player.TakeDamage(transform, enemySO.enemyDamageAmount);
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
            
            if (dissolveEffectPrefab != null)
            {
                Instantiate(dissolveEffectPrefab, transform.position, Quaternion.identity);
            }

            StartCoroutine(DestroyAfterDelay(1.5f));
        }
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}