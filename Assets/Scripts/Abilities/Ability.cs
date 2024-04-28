using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability
{
    public string abilityName;
    public Sprite icon; // If you want to display the ability in the UI
    public float cooldown;
    public CharacterBase abilityUser;
    


    public abstract void Activate(CharacterBase abilityUser);
    // Add more common functionality and properties that all abilities should have.
}
