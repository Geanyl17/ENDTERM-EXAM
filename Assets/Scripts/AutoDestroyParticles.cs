using UnityEngine;

public class AutoDestroyParticles : MonoBehaviour
{
    void Update()
    {
        var ps = GetComponent<ParticleSystem>();
        if (ps && !ps.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
