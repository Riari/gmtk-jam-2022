using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : MonoBehaviour
{
    public bool IsSelected { get; private set; }

    private GameObject _highlight;
    private GameObject _anchor; // Used to lock the player to a cell position

    void Start()
    {
        IsSelected = false;
        _highlight = GameObject.Find("Highlight");
        _highlight.SetActive(false);

        _anchor = GameObject.Find("Anchor");
    }

    void Update()
    {
        _highlight.SetActive(IsSelected);
    }

    public Vector3 GetAnchorPosition()
    {
        return _anchor.transform.position;
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
