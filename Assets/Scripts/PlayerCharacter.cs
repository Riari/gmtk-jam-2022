using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : MonoBehaviour
{
    [field: SerializeField]
    public float SecondsPerMove { get; set; } = 1.0f;

    [field: SerializeField]
    public GameObject Grid { get; set; }

    public bool IsSelected { get; private set; }
    public bool IsMoving { get; private set; }

    private Grid _grid;
    private GameObject _highlight;
    private GameObject _anchor; // Used to lock the player to a cell position
    private Animator _animator;

    private Queue<Cell> _path;

    private Vector3 _prevPosition;
    private Vector3 _nextPosition;
    private float _moveTimer = 0f;

    void Start()
    {
        IsSelected = false;

        _grid = Grid.GetComponent<Grid>();

        _highlight = GameObject.Find("Highlight");
        _highlight.SetActive(false);

        _anchor = GameObject.Find("Anchor");
        _animator = GameObject.Find("Sprite").GetComponent<Animator>();
    }

    void Update()
    {
        _highlight.SetActive(IsSelected);

        if (_moveTimer > 0f)
        {
            transform.position = Vector3.Lerp(_nextPosition, _prevPosition, Mathf.InverseLerp(0f, SecondsPerMove, _moveTimer));
            _moveTimer -= Time.deltaTime;
        }
        else
        {
            if (_path != null && _path.Count > 0)
            {
                IsMoving = true;
                _prevPosition = transform.position;
                _nextPosition = _grid.GetCellCenterWorld(_path.Dequeue().Position);
                _moveTimer = SecondsPerMove;
            }
            else
            {
                IsMoving = false;
            }
        }

        _animator.SetBool("IsMoving", IsMoving);
    }

    public void Move(List<Cell> path)
    {
        _path = new Queue<Cell>(path);
    }

    public void StopMoving()
    {
        _path = null;
        _moveTimer = 0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        StopMoving();
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
