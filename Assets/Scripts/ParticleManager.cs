using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ParticleManager : MonoBehaviour
{
    string PathToPrefabs = "Prefabs/Particles/";

    
    public static ParticleManager Instance;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of ParticleManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SpawnParticleEffect(string particlePrefabPath, Vector3 position, Quaternion rotation)
    {
        GameObject particlePrefab = Resources.Load<GameObject>(PathToPrefabs + particlePrefabPath);
        if (particlePrefab != null)
        {
            Instantiate(particlePrefab, position, rotation);
        }
        else
        {
            Debug.LogError("Particle prefab is not assigned.");
        }
    }
}
