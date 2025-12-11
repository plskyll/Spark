using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement; 
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Налаштування UI")]
    [SerializeField] private TextMeshProUGUI scoreText; 
    [SerializeField] private GameObject winScreen; 

    private int totalFragments;
    private int collectedFragments;

    private int totalEnemies;
    private int deadEnemies;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Більше ніж один GameManager у сцені!");
        }
        Instance = this;
    }

    private void Start()
    {
        totalFragments = FindObjectsOfType<PickFragment>().Length;
        collectedFragments = 0;

        EnemyEntity[] enemies = FindObjectsOfType<EnemyEntity>();
        totalEnemies = enemies.Length;
        deadEnemies = 0;

        foreach (EnemyEntity enemy in enemies)
        {
            enemy.OnDeath += OnEnemyKilled;
        }
        
        UpdateScoreText();
        
        if (winScreen != null) 
            winScreen.SetActive(false);
    }

    public void FragmentCollected()
    {
        collectedFragments++;
        UpdateScoreText();
        CheckWinCondition();
    }

    private void OnEnemyKilled(object sender, EventArgs e)
    {
        deadEnemies++;
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        if (collectedFragments >= totalFragments || deadEnemies >= totalEnemies)
        {
            WinGame();
        }
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Уламків: {collectedFragments} / {totalFragments}";
        }
        else
        {
            Debug.Log($"Зібрано: {collectedFragments} / {totalFragments}, Вбито: {deadEnemies} / {totalEnemies}");
        }
    }

    private void WinGame()
    {
        Debug.Log("ВІТАЮ! ВИ ПЕРЕМОГЛИ!");
        Time.timeScale = 0f;
        if (winScreen != null)
            winScreen.SetActive(true); 
    }
    
    private void OnDestroy()
    {
        EnemyEntity[] enemies = FindObjectsOfType<EnemyEntity>();
        foreach (EnemyEntity enemy in enemies)
        {
            if(enemy != null) enemy.OnDeath -= OnEnemyKilled;
        }
    }
}