using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballAbility : Ability
{
    public override void Activate(CharacterBase abilityUser)
    {
        // Fireball ability logic here
    }


}

public class HealAbility : Ability
{
    public float manaCost = 10f;
    public float healAmount = 50f; // The amount of health to restore
    TargetingSystem targetingSystem = TargetingSystem.Instance;

    public override void Activate(CharacterBase abilityUser)
    {
        // Find the TargetingSystem component on the player or wherever it's attached
        CharacterBase target = targetingSystem.CurrentTarget;

        if (abilityUser.mana >= manaCost)
        {
            if (target != null)
            {
                target.GetComponent<CharacterBase>().Heal(healAmount);// Perform actions with the target...
                abilityUser.useMana(manaCost);
            }
        }
    }
}