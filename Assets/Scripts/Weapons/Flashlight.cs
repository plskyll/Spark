using System;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private int damageAmount = 2;
    public event EventHandler OnFlashlightOn;

    private PolygonCollider2D polygonCollider2D;
    
    private void Awake()
    {
        polygonCollider2D = GetComponent<PolygonCollider2D>();
    }
    
    private void Start()
    {
        AttackColliderTurnOff();
    }
    
    public void Attack()
    {
        AttackColliderTurnOffOn();
        OnFlashlightOn?.Invoke(this, EventArgs.Empty);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out EnemyEntity enemyEntity))
        {
            enemyEntity.TakeDamage(damageAmount);
        }
    }

    public void AttackColliderTurnOff()
    {
        polygonCollider2D.enabled = false;
    }

    private void AttackColliderTurnOn()
    {
        polygonCollider2D.enabled = true;
    }

    private void AttackColliderTurnOffOn()
    {
        AttackColliderTurnOff();
        AttackColliderTurnOn();
    }
}
