using UnityEngine;

public class Friend : MonoBehaviour
{
    [field: SerializeField]
    public Events.CombatStartedEvent OnCombatInitiated;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Enemy") return;

        OnCombatInitiated.Invoke(gameObject, other.gameObject);
    }

    public void Kill()
    {
    }
}
