using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GoalManagement : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private CanvasGroup victoryCanvasGroup;

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 1.5f;
    private bool triggered = false;

    public void TriggerVictory()
    {
        if (triggered)
            return;

        triggered = true;

        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            victoryCanvasGroup.alpha =
                Mathf.Clamp01(t / fadeDuration);

            yield return null;
        }
        victoryCanvasGroup.alpha = 1f;
    }
}