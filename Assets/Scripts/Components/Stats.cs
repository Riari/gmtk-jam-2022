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

    public Tuple<int, int> Attack = new(1, Dice.D6);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
