using System;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [field: SerializeField]
    public Events.CharacterStatsInitialisedEvent OnInitialised;

    [field: SerializeField]
    public Events.CharacterHealthChangedEvent OnHealthChanged;

    [field: SerializeField]
    public int Health = 20;

    [field: SerializeField]
    public int MaxHealth = 20;
    
    [field: SerializeField]
    public int AC = 12;

    public Tuple<int, int> MeleeDamage = new(2, Dice.D6);
    public Tuple<int, int> MagicDamage = new(1, Dice.D4);

    private void Start()
    {
        OnInitialised.Invoke(this);
    }

    public void Damage(int amount)
    {
        Health -= amount;
        if (Health < 0) Health = 0;
        OnHealthChanged.Invoke(Health);
    }
}
