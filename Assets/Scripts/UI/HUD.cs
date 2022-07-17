using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    [field: SerializeField]
    public GameObject CombatLog;

    [field: SerializeField]
    public GameObject AttackModeContainer;

    [field: SerializeField]
    public GameObject CombatActions;

    private TextMeshProUGUI _combatLog;

    public bool MagicAttackUsed = false;

    private void Start()
    {
        AttackModeContainer.SetActive(false);

        _combatLog = CombatLog.GetComponent<TextMeshProUGUI>();

        DisableCombatActions();
    }

    public void EnableCombatActions()
    {
        for (int i = 0; i < CombatActions.transform.childCount; i++)
        {
            var obj = CombatActions.transform.GetChild(i).gameObject;
            
            // TODO: Find a better way of managing magic usage/cost
            if (obj.name == "MagicMissile" && MagicAttackUsed) continue;

            SetActionEnabled(obj, true);
        }
    }

    public void DisableCombatActions()
    {
        for (int i = 0; i < CombatActions.transform.childCount; i++)
        {
            SetActionEnabled(CombatActions.transform.GetChild(i).gameObject, false);
        }
    }

    private void SetActionEnabled(GameObject actionObject, bool enabled)
    {
        var action = actionObject.gameObject;

        var image = action.GetComponent<Image>();
        var color = image.color;
        color.a = enabled ? 1.0f : 0.8f;
        image.color = color;

        var button = action.GetComponent<Button>();
        button.interactable = enabled;
    }

    public void AddCombatLogEntry(string entry)
    {
        _combatLog.text += entry + "\n";
    }

    public void OnCombatInitiated(GameObject attacker, GameObject target)
    {
        AttackModeContainer.SetActive(true);
    }
}
