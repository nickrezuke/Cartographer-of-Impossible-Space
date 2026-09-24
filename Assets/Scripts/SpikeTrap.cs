using System.Collections;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] private float downTime = 2f;
    [SerializeField] private float upTime = 1f;
    [SerializeField] private float startDelay = 0f;

    [Header("Movement")]
    [SerializeField] private Vector3 riseDirection = Vector3.up;
    [SerializeField] private float riseHeight = -1f;
    [SerializeField] private float moveSpeed = 2f;

    [Header("Damage")]
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private float damageRange = 1.2f;

    private bool spikesUp = false;

    private Vector3 downPosition;
    private Vector3 upPosition;

    void Start()
    {
        downPosition = transform.localPosition;
        upPosition = downPosition + riseDirection.normalized * riseHeight;

        StartCoroutine(SpikeCycle());
    }

    private IEnumerator SpikeCycle()
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            // Spikes visually go up here.
            yield return MoveSpikes(downPosition, true);

            DamagePlayerIfClose();

            yield return new WaitForSeconds(upTime);

            // Spikes visually go down here.
            yield return MoveSpikes(upPosition, false);

            yield return new WaitForSeconds(downTime);
        }
    }

    private IEnumerator MoveSpikes(Vector3 targetPosition, bool active)
    {
        while (Vector3.Distance(transform.localPosition, targetPosition) > 0.01f)
        {
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition,
                targetPosition,
                moveSpeed * Time.deltaTime);

            yield return null;
        }

        transform.localPosition = targetPosition;
        spikesUp = active;
    }

    private void DamagePlayerIfClose()
    {
        if (playerHealth == null)
            return;

        Vector2 playerXZ = new Vector2(
            playerHealth.transform.position.x,
            playerHealth.transform.position.z);

        Vector2 spikeXZ = new Vector2(
            transform.position.x,
            transform.position.z);

        float distance = Vector2.Distance(playerXZ, spikeXZ);

        Debug.Log(gameObject.name + " XZ distance to player: " + distance);

        if (distance <= damageRange)
        {
            playerHealth.TakeDamage(damageAmount);
        }
    }

    public bool IsUp()
    {
        return spikesUp;
    }
}