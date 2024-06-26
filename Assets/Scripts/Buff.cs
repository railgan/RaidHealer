using UnityEngine;
using System;

[Serializable]
public class Buff
{
    public string name;
    public float duration;
    public Action<CharacterBase> ApplyEffect;
    public Action<CharacterBase> RemoveEffect;
    private float remainingDuration;
    private int id;

    public Buff(string name, float duration, Action<CharacterBase> applyEffect, Action<CharacterBase> removeEffect)
    {
        this.name = name;
        this.duration = duration;
        this.ApplyEffect = applyEffect;
        this.RemoveEffect = removeEffect;
        this.remainingDuration = duration;
        this.id = BuffManager.Instance.GetNextBuffId();
    }

    public bool Update(float deltaTime)
    {
        remainingDuration -= deltaTime;
        return remainingDuration <= 0;
    }

    public float GetRemainingDuration()
    {
        return remainingDuration;
    }

    public void RefreshDuration()
    {
        remainingDuration = duration;
    }

    public int GetID()
    {
        return id;
    }
}
