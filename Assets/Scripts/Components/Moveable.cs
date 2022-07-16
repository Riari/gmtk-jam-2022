using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour
{
    [field: SerializeField]
    public float SecondsPerMove { get; set; } = 0.2f;

    [field: SerializeField]
    public GameObject Grid { get; set; }

    public bool IsMoving { get; private set; }

    private Grid _grid;

    private SpriteRenderer _spriteRenderer;

    private Queue<Cell> _path;

    private Vector3 _prevPosition;
    private Vector3 _nextPosition;
    private float _moveTimer = 0f;

    private Animator _animator;

    private void Start()
    {
        _grid = Grid.GetComponent<Grid>();
        var sprite = GameObject.Find("Sprite");
        _spriteRenderer = sprite.GetComponent<SpriteRenderer>();
        _animator = sprite.GetComponent<Animator>();
    }

    private void Update()
    {
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
                _spriteRenderer.flipX = _prevPosition.x > _nextPosition.x;
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
        if (other.gameObject.tag != "TeleportTrigger") return;

        StopMoving();
    }
}
