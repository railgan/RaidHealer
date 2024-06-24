using UnityEngine;

public class HasteBuffAbility : Ability
{
    public float manaCost = 10f;
    public float hasteAmount = 20f; // The amount of haste to apply
    TargetingSystem targetingSystem = TargetingSystem.Instance;
    public string hasteEffectPrefabName = "HasteEffect"; // Name of the haste effect prefab in Resources folder

    public Color startBeamColor = Color.yellow;
    public Color endBeamColor = new Color(1f, 1f, 0f, 0.5f); // Fades to half transparent yellow
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
                Buff hasteBuff = new Buff(
                    "Haste",
                    5f,
                    (c) => c.haste += hasteAmount,
                    (c) => c.haste -= hasteAmount
                );
                target.GetComponent<CharacterBase>().ApplyBuff(hasteBuff);
                abilityUser.useMana(manaCost);

                // Use ParticleManager to draw the yellow beam
                if (ParticleManager.Instance != null)
                {
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
