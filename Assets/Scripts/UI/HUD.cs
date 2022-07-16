using UnityEngine;

public class HUD : MonoBehaviour
{
    private GameObject _attackModeContainer;

    void Start()
    {
        _attackModeContainer = GameObject.Find("AttackMode");
        _attackModeContainer.SetActive(false);
    }

    public void OnCombatInitiated()
    {
        _attackModeContainer.SetActive(true);
    }
}
