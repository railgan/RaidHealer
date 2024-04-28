using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    public LayerMask targetLayer; // Assign this in the Inspector
    private CharacterBase currentTarget; // Store the current target

    public static TargetingSystem Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        // Update currentTarget every frame
        currentTarget = GetCurrentTarget();
               
    }

    // Public method to access the currentTarget
    public CharacterBase GetCurrentTarget()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, targetLayer);

        if (hit.collider != null)
        {
            return hit.collider.GetComponent<CharacterBase>();
        }
        
        return null;
    }

    // Public property to access the currentTarget
    public CharacterBase CurrentTarget
    {
        get { return currentTarget; }
    }
}
