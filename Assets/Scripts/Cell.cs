using UnityEngine;

public class Cell
{
    public Vector3Int Position { get; set; }
    public int Cost { get; set; }
    public int Distance { get; set; }
    public int CostDistance => Cost + Distance;
    public Cell Parent { get; set; }
}
