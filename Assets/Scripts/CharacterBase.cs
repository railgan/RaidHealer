using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public float mana = 100f;
    public float maxMana = 100f;
    public float health = 100f;
    public float maxHealth = 100f;
    public float movementSpeed = 5f;
    public GameObject healthBarPrefab;
    private HealthBar healthBar;
    private ManaBar manaBar;
    private List<Buff> activeBuffs = new List<Buff>();

    protected virtual void Start()
    {
        // Ensure the healthBarPrefab is assigned.
        if (healthBarPrefab == null)
        {
            Debug.LogError("HealthBar prefab has not been assigned in the Inspector.");
            return;
        }

        // Instantiate the health bar prefab as a child of this character.
        GameObject healthBarObj = Instantiate(healthBarPrefab, transform);
        healthBarObj.name = "HealthBar";

        // Get the renderer component from the character
        Renderer characterRenderer = GetComponentInChildren<Renderer>();
        if (characterRenderer == null)
        {
            Debug.LogError("No renderer found on the character for dynamic health bar positioning.");
            return;
        }

        // Calculate the top center position of the character renderer.
        Vector3 topCenterPosition = new Vector3(
            characterRenderer.bounds.center.x,
            characterRenderer.bounds.max.y,
            characterRenderer.bounds.center.z
        );

        // Adjust the position of the health bar above the character.
        healthBarObj.transform.position = topCenterPosition + new Vector3(0, 0.4f, 0); // Adjust the y-value as needed.

        // Get the HealthBar component and set the health.
        healthBar = healthBarObj.GetComponentInChildren<HealthBar>();
        if (healthBar == null)
        {
            Debug.LogError("HealthBar component not found on the prefab.");
            return;
        }
        healthBar.SetHealth(health / maxHealth);
        
        manaBar = healthBarObj.GetComponentInChildren<ManaBar>();
        manaBar.SetMana(mana / maxMana);

    }

    public void Heal(float amount)
    {
        // Add the specified amount to health, making sure not to exceed maxHealth
        health += amount;
        health = Mathf.Min(health, maxHealth); // Ensure health does not go over maxHealth

        // Update the health bar, if applicable
        if (healthBar != null)
        {
            healthBar.SetHealth(health / maxHealth);
        }

        Debug.Log(gameObject.name + " healed for " + amount + ". Current health: " + health);
    }
    public void ApplyBuff(Buff buff)
    {
        buff.ApplyEffect?.Invoke(this);
        activeBuffs.Add(buff);
    }

    public void RemoveBuff(Buff buff)
    {
        buff.RemoveEffect?.Invoke(this);
        activeBuffs.Remove(buff);
    }

    public void useMana(float amount)
    {
        mana -= amount;
        mana = Mathf.Min(mana,maxMana);

        if (manaBar != null)
        {
            manaBar.SetMana(mana /maxMana);
        }
    }

    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        healthBar.SetHealth(health / maxHealth);
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        // Handle the death of the character
        // This could be destroying the GameObject, playing an animation, etc.
        Debug.Log(gameObject.name + " died.");
        Destroy(gameObject);
    }

    protected virtual void Update()
    {
        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            if (activeBuffs[i].Update(Time.deltaTime))
            {
                RemoveBuff(activeBuffs[i]);
            }
        }
    }
}