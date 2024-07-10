using UnityEngine;

public class AbilityShape : MonoBehaviour
{
    public float damage = 90f;
    public float delay = 2.0f;
    public string targetTag = "Enemy"; // Default target

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

        // Starte die Verzögerung
        Invoke(nameof(DoDamage), delay);
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

        // Optional: Zerstöre den Schadenskreis nach dem Zufügen von Schaden
        Destroy(gameObject, 0.5f);
    }

    // Zeichne den Radius im Editor, um die Größe des Schadenskreises zu visualisieren
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
