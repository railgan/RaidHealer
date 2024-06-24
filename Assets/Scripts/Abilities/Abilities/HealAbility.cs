using UnityEngine;

public class HealAbility : Ability
{
    public float manaCost = 10f;
    public float healAmount = 50f; // The amount of health to restore
    TargetingSystem targetingSystem = TargetingSystem.Instance;
    public string healingEffectPrefabName = "HealingEffect"; // Name of the healing effect prefab in Resources folder

    public Color startBeamColor = Color.green;
    public Color endBeamColor = new Color(0f, 1f, 0f, 0.5f); // Fades to half transparent green
    public float beamWidth = 0.1f;
    public float beamDuration = 0.5f; // Duration the beam will be visible

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
                    ParticleManager.Instance.DrawBeam(abilityUser.transform, target.transform.position, startBeamColor, endBeamColor, beamWidth, beamDuration);
                }
                else
                {
                    Debug.LogError("ParticleManager instance is not found.");
                }
            }
        }
    }
}
