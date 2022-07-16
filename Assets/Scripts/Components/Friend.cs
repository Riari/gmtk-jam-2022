using UnityEngine;
using UnityEngine.Events;

public class Friend : MonoBehaviour
{
    [field: SerializeField]
    public Events.CombatEvent OnCombatInitiated;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Enemy") return;

        OnCombatInitiated.Invoke(gameObject, other.gameObject);
    }
}
