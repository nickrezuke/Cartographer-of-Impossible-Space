using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 6;
    [SerializeField] private Renderer antRenderer;
    [SerializeField] private float flashTime = 0.15f;

    private int currentHealth;
    private Color originalColor;
    private bool isFlashing = false;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;

        if (antRenderer != null)
        {
            originalColor = antRenderer.material.color;
        }

        Debug.Log("Health: " + currentHealth);
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead)
            return;

        currentHealth -= damageAmount;

        if (currentHealth < 0)
            currentHealth = 0;

        Debug.Log("Player took damage. Health: " + currentHealth);

        if (!isFlashing)
        {
            StartCoroutine(FlashRed());
        }

        if (currentHealth <= 0)
        {
            isDead = true;
            StartCoroutine(RestartAfterDeath());
        }
    }

    private IEnumerator FlashRed()
    {
        if (antRenderer == null)
            yield break;

        isFlashing = true;

        antRenderer.material.color = Color.red;
        yield return new WaitForSeconds(flashTime);

        antRenderer.material.color = originalColor;

        isFlashing = false;
    }

    private IEnumerator RestartAfterDeath()
    {
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }
}