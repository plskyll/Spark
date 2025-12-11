using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;

    private void Start()
    {
        if (Player.Instance != null)
        {
            Player.Instance.OnHealthChanged += Player_OnHealthChanged;
        }
    }

    private void Player_OnHealthChanged(object sender, float healthPercent)
    {
        healthBarImage.fillAmount = healthPercent;
    }
}