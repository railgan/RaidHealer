using UnityEngine;

public class HealAbility : Ability
{
    public float manaCost = 10f;
    public float healAmount = 50f; // The amount of health to restore
    TargetingSystem targetingSystem = TargetingSystem.Instance;
    public string healingEffectPrefabName = "HealingEffect"; // Name of the healing effect prefab in Resources folder


    public override void Activate(CharacterBase abilityUser)
    {
        // Find the TargetingSystem component on the player or wherever it's attached
        CharacterBase target = targetingSystem.CurrentTarget;

        if (abilityUser.mana >= manaCost)
        {
            if (target != null)
            {
                target.GetComponent<CharacterBase>().Heal(healAmount); // Perform actions with the target...
                abilityUser.useMana(manaCost);

                // Use ParticleManager to spawn the healing effect
                if (ParticleManager.Instance != null)
                {
                    ParticleManager.Instance.SpawnParticleEffect(healingEffectPrefabName, target.transform.position, target.transform.rotation);
                }
                else
                {
                    Debug.LogError("ParticleManager instance is not found.");
                }
            }
        }
    }
}
