using UnityEngine;

public class Torches : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem activateParticles;

    public void Ignite()
    {
        if (!activateParticles.isPlaying)
            activateParticles.Play();
    }
}