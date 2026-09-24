using UnityEngine;

public class GoalAccomplish : MonoBehaviour
{
    [SerializeField] private GoalManagement victoryManager;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("GOAL HIT");
        if (!other.CompareTag("Player"))
            return;
        victoryManager.TriggerVictory();
    }
}