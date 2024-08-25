using UnityEngine;

public class HasteBuffAbility : Ability
{
    public float manaCost = 10f;
    public float hasteAmount = 20f;
    TargetingSystem targetingSystem = TargetingSystem.Instance;

    public string hasteEffectPrefabName = "HasteEffect";

    public Color startBeamColor = Color.yellow;
    public Color endBeamColor = new Color(1f, 1f, 0f, 0.5f);
    public float beamWidth = 0.1f;
    public float beamDuration = 0.5f;

    public override void Activate(CharacterBase abilityUser)
    {
        CharacterBase target = targetingSystem.CurrentTarget;

        if (abilityUser.mana >= manaCost)
        {
            if (target != null)
            {
                Buff hasteBuff = new Buff(
                    "Haste",
                    5f,
                    (c) => {
                        c.haste += hasteAmount;
                    },
                    (c) => {
                        c.haste -= hasteAmount;
                    }
                );
                target.GetComponent<CharacterBase>().ApplyBuff(hasteBuff);
                abilityUser.useMana(manaCost);

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
