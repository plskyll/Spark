using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Налаштування UI")]
    [SerializeField] private TextMeshProUGUI scoreText; 
    [SerializeField] private GameObject winScreen; 

    private int totalFragments;
    private int collectedFragments;

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
        
        UpdateScoreText();
        
        if (winScreen != null) 
            winScreen.SetActive(false);
    }

    public void FragmentCollected()
    {
        collectedFragments++;
        UpdateScoreText();

        // Перевірка умови перемоги
        if (collectedFragments >= totalFragments)
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
            Debug.Log($"Зібрано: {collectedFragments} / {totalFragments}");
        }
    }

    private void WinGame()
    {
        Debug.Log("ВІТАЮ! ВИ ПЕРЕМОГЛИ!");
        
        if (winScreen != null)
            winScreen.SetActive(true); 
    }
}