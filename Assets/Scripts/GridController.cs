using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class GridController : MonoBehaviour
{
    [field: SerializeField]
    public GameObject Player { get; set; }

    [System.Serializable]
    public class CellHighlight
    {
        public GameObject Overlay;
        public Color ValidColor;
        public Color InvalidColor;

        public void Enable() => Overlay.SetActive(true);
        public void Disable() => Overlay.SetActive(false);

        public void SetPosition(Vector3 position) => Overlay.transform.position = position;

        public void SetValid(bool isValid)
        {
            Overlay.GetComponent<SpriteRenderer>().color = isValid ? ValidColor : InvalidColor;
        }
    }
    public CellHighlight cellHighlight;

    [field: Header("Tilemaps")]

    [field: SerializeField]
    public GameObject WalkableTilemap { get; set; }

    [field: SerializeField]
    public List<GameObject> NonWalkableTilemaps { get; set; }

    private Grid _grid;
    private PlayerCharacter _player;
    private Tilemap _walkableTilemap;
    readonly private List<Tilemap> _nonWalkableTilemaps = new List<Tilemap>();
    private Vector2 _cursorPosition;

    private Vector3Int _destination;
    private bool _isValidDestination;

    private List<Cell> _path = null;

    readonly private HashSet<Vector3Int> _walkableCells = new HashSet<Vector3Int>();

    void Start()
    {
        _grid = GetComponent<Grid>();
        _player = Player.GetComponent<PlayerCharacter>();
        _walkableTilemap = WalkableTilemap.GetComponent<Tilemap>();
        NonWalkableTilemaps.ForEach(map => _nonWalkableTilemaps.Add(map.GetComponent<Tilemap>()));

        // Cache walkable cell coordinates       
        foreach (Vector3Int position in _walkableTilemap.cellBounds.allPositionsWithin)
        {
            var tile = _walkableTilemap.GetTile(position);
            if (tile == null) continue;
            _walkableCells.Add(position);
        }

        _nonWalkableTilemaps.ForEach(map =>
        {
            foreach (Vector3Int position in map.cellBounds.allPositionsWithin)
            {
                if (!_walkableCells.Contains(position)) continue;
                var tile = map.GetTile(position);
                if (tile == null) continue;
                _walkableCells.Remove(position);
            }
        });
    }

    void Update()
    {
        if (!_player.IsSelected)
        {
            cellHighlight.Disable();
            return;
        }

        cellHighlight.Enable();
        _destination = _walkableTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(_cursorPosition));
        cellHighlight.SetPosition(_grid.GetCellCenterWorld(_destination));
        Vector3Int start = GetPlayerCell();
        _isValidDestination = start.ManhattanDistance(_destination) < 5;
        cellHighlight.SetValid(_isValidDestination);
    }

    Vector3Int GetPlayerCell()
    {
        return _walkableTilemap.WorldToCell(Player.transform.position);
    }

    public void OnCursorPositionAction(InputAction.CallbackContext context)
    {
        _cursorPosition = Mouse.current.position.ReadValue();
    }

    public void OnSecondaryAction(InputAction.CallbackContext context)
    {
        if (!_isValidDestination || !_player.IsSelected) return;

        _path = CalculatePathTo(_destination);
    }

    List<Cell> CalculatePathTo(Vector3Int destination)
    {
        Vector3Int player = GetPlayerCell();

        var start = new Cell();
        start.Position = player;
        start.Distance = player.ManhattanDistance(destination);

        var end = new Cell();
        end.Position = destination;

        var openCells = new List<Cell>();
        openCells.Add(start);
        var closedCells = new List<Cell>();

        while (openCells.Any())
        {
            var next = openCells.OrderBy(cell => cell.CostDistance).First();

            if (next.Position == end.Position)
            {
                // Destination reached - return the path
                var path = new List<Cell>();
                path.Add(next);
                while (next.Parent != null && next.Position != start.Position)
                {
                    path.Add(next.Parent);
                    next = next.Parent;
                }

                path.Add(next);
                path.Reverse();
                return path;
            }

            closedCells.Add(next);
            openCells.Remove(next);

            foreach (var cell in GetWalkableCellsToDestination(next, end))
            {
                if (closedCells.Any(c => c.Position == cell.Position)) continue;

                if (openCells.Any(c => c.Position == cell.Position))
                {
                    var existing = openCells.First(c => c.Position == cell.Position);
                    if (existing.CostDistance > cell.CostDistance)
                    {
                        openCells.Remove(existing);
                        openCells.Add(cell);
                    }
                }
                else
                {
                    openCells.Add(cell);
                }
            }
        }

        // No valid path found
        return null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(Player.transform.position, 0.25f);

        if (_path == null) return;

        Gizmos.color = Color.green;
        _path.ForEach(cell =>
        {
            Gizmos.DrawSphere(_walkableTilemap.GetCellCenterWorld(cell.Position), 0.25f);
        });
    }

    private List<Cell> GetWalkableCellsToDestination(Cell current, Cell end)
    {
        var candidates = new List<Cell>()
        {
            new Cell { Position = new Vector3Int(current.Position.x, current.Position.y - 1), Parent = current, Cost = current.Cost + 1 },
            new Cell { Position = new Vector3Int(current.Position.x, current.Position.y + 1), Parent = current, Cost = current.Cost + 1 },
            new Cell { Position = new Vector3Int(current.Position.x - 1, current.Position.y), Parent = current, Cost = current.Cost + 1 },
            new Cell { Position = new Vector3Int(current.Position.x + 1, current.Position.y), Parent = current, Cost = current.Cost + 1 },
        };

        candidates.ForEach(cell => cell.Distance = cell.Position.ManhattanDistance(end.Position));

        var minX = _walkableTilemap.cellBounds.xMin;
        var minY = _walkableTilemap.cellBounds.yMin;
        var maxX = _walkableTilemap.cellBounds.xMax;
        var maxY = _walkableTilemap.cellBounds.yMax;

        return candidates
                .Where(cell => _walkableTilemap.cellBounds.Contains(cell.Position))
                .Where(cell => _walkableCells.Contains(cell.Position))
                .ToList();
    }

    class Cell
    {
        public Vector3Int Position { get; set; }
        public int Cost { get; set; }
        public int Distance { get; set; }
        public int CostDistance => Cost + Distance;
        public Cell Parent { get; set; }
    }
}