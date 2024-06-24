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

    public void DrawBeam(Transform abilityUser, Vector3 end, Color startColor, Color endColor, float beamWidth, float beamDuration)
    {
        GameObject beamObject = new GameObject("HealingBeam");
        LineRenderer lineRenderer = beamObject.AddComponent<LineRenderer>();

        lineRenderer.startWidth = beamWidth;
        lineRenderer.endWidth = beamWidth;
        lineRenderer.material = new Material(Shader.Find("Custom/ScrollingBeamShader"));
        lineRenderer.material.SetColor("_StartColor", startColor);
        lineRenderer.material.SetColor("_EndColor", endColor);
        lineRenderer.material.SetFloat("_ScrollSpeed", 2f); // Adjust scroll speed as needed
        lineRenderer.material.SetFloat("_FadeLength", 0.5f); // Adjust fade length as needed

        lineRenderer.SetPosition(0, abilityUser.position);
        lineRenderer.SetPosition(1, end);

        // Start a coroutine to update the beam position
        StartCoroutine(UpdateBeamPosition(abilityUser, lineRenderer, end, beamDuration));
    }

    private System.Collections.IEnumerator UpdateBeamPosition(Transform abilityUser, LineRenderer lineRenderer, Vector3 end, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            if (lineRenderer != null)
            {
                lineRenderer.SetPosition(0, abilityUser.position);
                lineRenderer.SetPosition(1, end);
            }
            timer += Time.deltaTime;
            yield return null;
        }

        // Destroy the beam object after the duration
        if (lineRenderer != null)
        {
            Destroy(lineRenderer.gameObject);
        }
    }
}
