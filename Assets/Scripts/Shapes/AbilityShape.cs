using System;
using UnityEngine;

public class AbilityShape : MonoBehaviour
{
    public float damage = 90f;
    public float delay = 2.0f;
    public string targetTag = "Enemy"; // Default target
    public Boolean growing = true;
    private float startTime;
    public Vector3 targetScale = new Vector3(1f, 1f, 1f);
    private Vector3 initialScale;


    private CircleCollider2D circleCollider;

    void Start()
    {
               
        // Hole den CircleCollider2D vom GameObject
        circleCollider = GetComponent<CircleCollider2D>();
        if (circleCollider == null)
        {
            Debug.LogError("Kein CircleCollider2D auf diesem GameObject gefunden!");
            return;
        }

        startTime = Time.time;
        initialScale = transform.localScale;

        // Starte die Verzögerung
        Invoke(nameof(DoDamage), delay);
    }

    void Update()
    {
        float elapsedTime = Time.time - startTime;
       
        // Calculate the scaled size based on elapsed time and scale factor
        float scale = (1/delay*elapsedTime);

        // Clamp the scale to the target scale
        scale = Mathf.Clamp(scale, initialScale.x, targetScale.x);
        transform.localScale = new Vector3(scale, scale, scale);
        
    }

    private void DoDamage()
    {
        // Finde alle Collider im Radius des CircleColliders
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, circleCollider.radius);

        // Iteriere über alle gefundenen Collider
        foreach (var hitCollider in hitColliders)
        {
            // Überprüfe, ob der Collider das richtige Tag hat
            if (hitCollider.CompareTag(targetTag))
            {
                // Versuche, die CharacterBase-Komponente zu bekommen
                CharacterBase character = hitCollider.GetComponent<CharacterBase>();

                // Wenn die Komponente vorhanden ist, füge Schaden zu
                if (character != null)
                {
                    character.TakeDamage(damage);
                }
            }
        }
        
        Destroy(gameObject.transform.parent.gameObject);
        //Destroy(gameObject, 0.5f);
    }

    private void OnDrawGizmosSelected()
    {
        if (circleCollider == null)
        {
            circleCollider = GetComponent<CircleCollider2D>();
        }

        if (circleCollider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, circleCollider.radius);
        }
    }
}
