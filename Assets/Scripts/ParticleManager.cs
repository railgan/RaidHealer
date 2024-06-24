using UnityEngine;

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

    public void DrawHealingBeam(Vector3 start, Vector3 end, Color beamColor, float beamWidth, float beamDuration)
    {
        GameObject beamObject = new GameObject("HealingBeam");
        LineRenderer lineRenderer = beamObject.AddComponent<LineRenderer>();

        lineRenderer.startWidth = beamWidth;
        lineRenderer.endWidth = beamWidth;
        lineRenderer.material = new Material(Shader.Find("Custom/ScrollingBeamShader"));
        lineRenderer.material.SetColor("_Color", beamColor);
        lineRenderer.material.SetFloat("_ScrollSpeed", 2f); // Adjust scroll speed as needed

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        // Destroy the beam object after the duration
        Destroy(beamObject, beamDuration);
    }
}
