using UnityEngine;
using System.Collections;

public class FlashBlink : MonoBehaviour
{
    [SerializeField] private Material blinkMaterial;
    [SerializeField] private float blinkDuration = 0.2f;

    [SerializeField] private EnemyEntity enemyTarget;
    [SerializeField] private Player playerTarget;

    private SpriteRenderer[] spriteRenderers;
    private Material[] defaultMaterials;
    private Coroutine blinkCoroutine;

    private void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        if (spriteRenderers.Length == 0)
        {
            var parent = GetComponentInParent<Transform>();
            if (parent != null)
            {
                spriteRenderers = parent.GetComponentsInChildren<SpriteRenderer>();
            }
        }

        defaultMaterials = new Material[spriteRenderers.Length];
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            defaultMaterials[i] = spriteRenderers[i].material;
        }
    }

    private void Start()
    {
        if (enemyTarget == null && playerTarget == null)
        {
            enemyTarget = GetComponentInParent<EnemyEntity>();
            if (enemyTarget == null)
            {
                playerTarget = GetComponentInParent<Player>();
            }
        }

        if (enemyTarget != null)
        {
            enemyTarget.OnTakeHit += (sender, args) => Flash();
        }
        else if (playerTarget != null)
        {
            playerTarget.OnFlashBlink += (sender, args) => Flash();
        }
    }

    public void Flash()
    {
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        blinkCoroutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        if (blinkMaterial == null) yield break;

        foreach (var sr in spriteRenderers)
        {
            if (sr != null) sr.material = blinkMaterial;
        }

        yield return new WaitForSeconds(blinkDuration);

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            if (spriteRenderers[i] != null)
            {
                spriteRenderers[i].material = defaultMaterials[i];
            }
        }
    }

    private void OnDestroy()
    {
        if (enemyTarget != null)
        {
            enemyTarget.OnTakeHit -= (sender, args) => Flash();
        }
        else if (playerTarget != null)
        {
            playerTarget.OnFlashBlink -= (sender, args) => Flash();
        }
    }
}