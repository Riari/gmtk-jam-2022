using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : MonoBehaviour
{
    private GameObject _highlight;
    private bool _isSelected;

    void Start()
    {
        _highlight = GameObject.Find("Highlight");
        _highlight.SetActive(false);
    }

    void Update()
    {
        _highlight.SetActive(_isSelected);
    }

    public void OnPrimaryAction(InputAction.CallbackContext context)
    {
        _isSelected = false;

        // TODO: Try to use mouse position from context instead
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        if (hit.collider == null) return;
        if (hit.collider.gameObject.tag != "Player") return;

        _isSelected = true;
    }

    public void OnCancelAction(InputAction.CallbackContext context)
    {
        _isSelected = false;
    }
}
