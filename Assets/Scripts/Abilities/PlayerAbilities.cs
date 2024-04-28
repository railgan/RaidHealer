using System.Collections.Generic;
using UnityEngine;

public class AbilitySelection : MonoBehaviour
{
    private PlayerAbilities playerAbilities;

    void Start()
    {
        playerAbilities = FindObjectOfType<PlayerAbilities>();
        // Make sure to initialize your UI here with all available abilities
    }
    // Implement the rest of your UI logic
}

public class PlayerAbilities : MonoBehaviour
{
    private List<Ability> availableAbilities = new List<Ability>(); // All abilities the player could possibly have
    private Ability[] activeAbilities = new Ability[4]; // The abilities the player currently has equipped

    void Start()
    {
        // Initialize available abilities here or in another appropriate place
        availableAbilities.Add(new HealAbility()); // Add the Heal ability to the available pool

        // Directly equip the Heal ability for demonstration purposes
        EquipAbility(0, new HealAbility()); 
    }

    public void EquipAbility(int abilitySlot, Ability abilityToEquip)
    {
        if (abilitySlot < 0 || abilitySlot >= activeAbilities.Length)
        {
            Debug.LogError("Invalid ability slot.");
            return;
        }
        activeAbilities[abilitySlot] = abilityToEquip;
        // Update UI if necessary
    }

    public void ActivateAbility(int abilitySlot)
    {
        if (abilitySlot < 0 || abilitySlot >= activeAbilities.Length || activeAbilities[abilitySlot] == null)
        {
            Debug.LogError("Invalid ability slot or no ability equipped.");
            return;
        }
        activeAbilities[abilitySlot].Activate(GetComponent<CharacterBase>());
        // Start cooldown, disable ability usage until it's ready again, etc.
    }

    // Add methods for activating abilities, cooldown management, etc.
}