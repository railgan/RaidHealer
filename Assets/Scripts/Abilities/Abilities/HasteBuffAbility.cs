using UnityEngine;

public class HasteBuffAbility : Ability
{
    public float manaCost = 10f;
    public float hasteAmount = 20f; // The amount of health to restore
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
                Buff hasteBuff = new Buff(
                "Haste Buff",
                5f,
                (c) => c.haste += 20,
                (c) => c.haste -= 20

            );
                target.GetComponent<CharacterBase>().ApplyBuff(hasteBuff);
                abilityUser.useMana(manaCost);
              
            }
        }
    }
}
