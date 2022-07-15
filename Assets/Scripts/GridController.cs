using UnityEngine;
using UnityEngine.InputSystem;

public class GridController : MonoBehaviour
{
    [field: SerializeField]
    public GameObject Player { get; set; }

    [field: SerializeField]
    public GameObject TileHighlight { get; set; }

    private Grid _grid;
    private Vector2 _cursorPosition;

    void Start()
    {
        _grid = GetComponent<Grid>();
    }

    void Update()
    {
        Vector3Int cell = _grid.WorldToCell(Camera.main.ScreenToWorldPoint(_cursorPosition));
        TileHighlight.transform.position = _grid.GetCellCenterWorld(cell);
    }

    public void OnCursorPositionAction(InputAction.CallbackContext context)
    {
        _cursorPosition = Mouse.current.position.ReadValue();
    }

    public void OnSecondaryAction(InputAction.CallbackContext context)
    {
    }
}
