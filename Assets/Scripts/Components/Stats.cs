using System;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [field: SerializeField]
    public int Health = 100;

    [field: SerializeField]
    public int MaxHealth = 100;
    
    [field: SerializeField]
    public int AC = 12;

    public Tuple<int, int> MeleeDamage = new(2, Dice.D6);
    public Tuple<int, int> MagicDamage = new(2, Dice.D6);
}
