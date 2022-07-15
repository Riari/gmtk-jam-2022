using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : MonoBehaviour
{
    [field: SerializeField]
    public GameObject MainCamera { get; set; }

    private GameObject Highlight;

    private bool IsSelected;

    void Start()
    {
        Highlight = GameObject.Find("Highlight");
        Highlight.SetActive(false);
    }

    void Update()
    {
        Highlight.SetActive(IsSelected);
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
