using UnityEngine;
using UnityEngine.InputSystem;

public class Selectable : MonoBehaviour
{
    public bool IsSelectable { get; private set; } = true;
    public bool IsSelected { get; private set; }

    private Material _material;

    void Start()
    {
        _material = GameObject.Find("Sprite").GetComponent<SpriteRenderer>().material;
        IsSelected = false;
    }

    void Update()
    {
        _material.SetInt("_OutlineEnabled", IsSelected ? 1 : 0);
    }

    public void Freeze()
    {
        IsSelectable = false;
        IsSelected = false;
    }
    
    public void Unfreeze()
    {
        IsSelectable = true;
    }

    public void OnPrimaryAction(InputAction.CallbackContext context)
    {
        if (!IsSelectable) return;

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
