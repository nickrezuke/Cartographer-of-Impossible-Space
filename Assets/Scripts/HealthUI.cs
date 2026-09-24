using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private TMP_Text healthText;

    void Update()
    {
        if (playerHealth != null && healthText != null)
        {
            string hearts = "";

            for (int i = 0; i < playerHealth.GetCurrentHealth(); i++)
            {
                hearts += "♥ ";
            }

            healthText.text = hearts;
        }
    }
}