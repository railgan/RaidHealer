using UnityEngine;
using UnityEngine.TextCore.Text;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 90f;
    private Vector2 moveDirection;
    public string targetTag = "Enemy"; // Default target

    // Function to initialize the projectile's direction
    public void Initialize(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

        // Check if the projectile is out of the camera's view
        if (!IsInView())
        {
            Destroy(gameObject);
        }
    }

    bool IsInView()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        return onScreen;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check for collision with the correct target
        if (other.gameObject.CompareTag(targetTag))
        {
            // Try to get the CharacterBase component on the object that was hit
            CharacterBase character = other.gameObject.GetComponent<CharacterBase>();

            // Optionally, check for collision with the player
            if (character != null)
            {
                character.TakeDamage(damage);
            }

            // Destroy the projectile after it hits something
            Destroy(gameObject);
        }
    }
}
