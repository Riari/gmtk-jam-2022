using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    [field: SerializeField]
    public GameObject CombatLog;

    private GameObject _attackModeContainer;
    private TextMeshProUGUI _combatLog;

    void Start()
    {
        _attackModeContainer = GameObject.Find("AttackMode");
        //_attackModeContainer.SetActive(false);

        _combatLog = CombatLog.GetComponent<TextMeshProUGUI>();
    }

    public void AddCombatLogEntry(string entry)
    {
        _combatLog.text += entry + "\n";
    }

    public void OnCombatInitiated(GameObject attacker, GameObject target)
    {
        _attackModeContainer.SetActive(true);
    }
}
