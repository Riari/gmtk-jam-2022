using UnityEngine;
using UnityEngine.InputSystem;

public class Selectable : MonoBehaviour
{
    public bool IsSelected { get; private set; }

    private GameObject _highlight;

    void Start()
    {
        IsSelected = false;

        _highlight = GameObject.Find("Highlight");
        _highlight.SetActive(false);
    }

    void Update()
    {
        _highlight.SetActive(IsSelected);
    }

    public void OnPrimaryAction(InputAction.CallbackContext context)
    {
        IsSelected = false;

        // TODO: Try to use mouse position from context instead
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        if (hit.collider == null) return;
        if (hit.collider.gameObject.tag != "Player") return;

        IsSelected = true;
    }

    public void OnCancelAction(InputAction.CallbackContext context)
    {
        IsSelected = false;
    }
}
