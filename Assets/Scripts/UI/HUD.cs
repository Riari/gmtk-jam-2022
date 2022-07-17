using UnityEngine;
using UnityEngine.SceneManagement;
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

    [field: SerializeField]
    public Slider HealthSlider;

    [field: SerializeField]
    public TextMeshProUGUI HealthText;

    [field: SerializeField]
    public Image FlashOverlay;

    [field: SerializeField]
    public GameObject DeathScreen;

    private TextMeshProUGUI _combatLog;

    private void Start()
    {
        AttackModeContainer.SetActive(false);

        _combatLog = CombatLog.GetComponent<TextMeshProUGUI>();

        DisableCombatActions();

        DeathScreen.SetActive(false);
    }

    public void EnableCombatActions()
    {
        for (int i = 0; i < CombatActions.transform.childCount; i++)
        {
            var obj = CombatActions.transform.GetChild(i).gameObject;

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

    public void ClearCombatLog()
    {
        _combatLog.text = "";
    }

    public void OnCombatInitiated(GameObject attacker, GameObject defender)
    {
        AttackModeContainer.SetActive(true);
    }

    public void OnCombatEnded(GameObject winner, GameObject loser)
    {
        AttackModeContainer.SetActive(false);
    }

    public void OnCharacterStatsInitialised(Stats stats)
    {
        HealthText.text = stats.Health.ToString();
        HealthSlider.maxValue = stats.MaxHealth;
        HealthSlider.value = stats.Health;
    }

    public void OnCharacterHealthChanged(int health)
    {
        HealthText.text = health.ToString();
        HealthSlider.value = health;
    }

    public void OnRespawnButtonPressed()
    {
        SceneManager.LoadScene("Dungeon1");
    }

    public void OnExitButtonPressed()
    {
        SceneManager.LoadScene("MainTitleScene");
    }
}
