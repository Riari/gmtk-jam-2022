using UnityEngine;

public class HintManager : MonoBehaviour
{
    private GameObject _selectHint;
    private GameObject _moveHint;
    private GameObject _fightHint;

    void Start()
    {
        _selectHint = GameObject.Find("Select");
        _moveHint = GameObject.Find("Move");
        _fightHint = GameObject.Find("Fight");

        _moveHint.SetActive(false);
        _fightHint.SetActive(false);
    }

    public void OnCharacterSelected(GameObject character)
    {
        _selectHint.SetActive(false);
        _moveHint.SetActive(true);
    }

    public void OnCharacterMoving(GameObject character)
    {
        _moveHint.SetActive(false);
        _fightHint.SetActive(true);
    }

    public void OnCombatInitiated(GameObject attacker, GameObject defender)
    {
        _fightHint.SetActive(false);
    }
}
