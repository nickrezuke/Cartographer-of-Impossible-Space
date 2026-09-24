using UnityEngine;
public class AntFollower : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    public float heading;

    void LateUpdate()
    {
        transform.position = player.position;

        //transform.rotation =
        //    Quaternion.Euler(0, heading, 0);
    }
}