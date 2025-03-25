using System.Collections.Generic;
using UnityEngine;

// Cell class represents a single grid cell in the GameBoard.
public class Cell : MonoBehaviour {
    private ComboManager comboManager;

    public List<Cell> adjacentCells { get; private set; }
    public List<Cell> surroundingCells { get; private set; }
    public GameBoard parentBoard { get; private set; }
    
    private Item currItem;
    
    [HideInInspector] public int gridX, gridY;
    [HideInInspector] public Cell cellBelow;
    [HideInInspector] public bool isFillingCell;

    // Exposes the Item in the cell.
    public Item item {
        get => currItem;
        set {
            if (currItem == value) return;
            if (currItem != null && ReferenceEquals(currItem.Cell, this))
                currItem.Cell = null;
            currItem = value;
            if (currItem != null)
                currItem.Cell = this;
        }
    }

    // Sets up cell's position and references.
    public void InitializeCell(int x, int y, GameBoard board) {
        parentBoard = board;
        gridX = x; 
        gridY = y;
        
        transform.localPosition = new Vector3 (x, y);
        isFillingCell = gridY == parentBoard.boardRows - 1;
        
        UpdateNeighbours();
        UpdateSurroundings();
    }

    public void SetComboManager(ComboManager manager) {
        comboManager = manager;
    }

    // Finds immediate neighbors (Up, Down, Left, Right).
    private void UpdateNeighbours() {
        adjacentCells = GetNeighbors(DIRECTION.Up, DIRECTION.Down, DIRECTION.Left, DIRECTION.Right);
        cellBelow = GetNeighbor(DIRECTION.Down);
    }

    // Gathers all surrounding cells in eight directions.
    private void UpdateSurroundings() {
        surroundingCells = GetNeighbors(
            DIRECTION.Up, DIRECTION.UpRight, DIRECTION.Right, DIRECTION.DownRight,
            DIRECTION.Down, DIRECTION.DownLeft, DIRECTION.Left, DIRECTION.UpLeft
        );
    }

    // Get cell's neighbors according to given directions.
    private List<Cell> GetNeighbors(params DIRECTION[] directions) {
        var results = new List<Cell>();
        foreach (DIRECTION dir in directions) {
            Cell neighbor = GetNeighbor(dir);
            if (neighbor != null) {
                results.Add(neighbor);
            }
        }
        return results;
    }

    // Get cell's neighbor according to given direction.
    public Cell GetNeighbor(DIRECTION direction) {
        Vector2Int offset = directionOffsets[direction];
        int targetX = gridX + offset.x;
        int targetY = gridY + offset.y;

        if (!IsInsideBoard(targetX, targetY)) {
            return null;
        }

        return parentBoard.Cells[targetX, targetY];
    }

    // The dictionary mapping each direction to an (x, y) offset.
    private static readonly Dictionary<DIRECTION, Vector2Int> directionOffsets = new Dictionary<DIRECTION, Vector2Int> {
        { DIRECTION.None, new Vector2Int(0, 0) },
        { DIRECTION.Up, new Vector2Int(0, 1) },
        { DIRECTION.Down, new Vector2Int(0, -1) },
        { DIRECTION.Right, new Vector2Int(1, 0) },
        { DIRECTION.Left, new Vector2Int(-1, 0) },
        { DIRECTION.UpRight, new Vector2Int(1, 1) },
        { DIRECTION.UpLeft, new Vector2Int(-1, 1) },
        { DIRECTION.DownRight, new Vector2Int(1, -1) },
        { DIRECTION.DownLeft, new Vector2Int(-1, -1) },
    };

    private bool IsInsideBoard(int x, int y) {
        return x >= 0 && x < parentBoard.boardCols && y >= 0 && y < parentBoard.boardRows;
    }

    public void OnCellTapped() {
        if (item == null) { return; }

        switch (item.GetMatchType()) {
            case MATCH_TYPE.Special: // ROCKET
                if (comboManager != null) {
                    comboManager.TryExecute(this);
                }
                break;
            default: // CUBES
                GroupChecker.Instance.ExplodeMatchingCells(this);
                break;
        }
    }

    public Cell FindFallTarget() {
        if (cellBelow != null && cellBelow.item == null) {
            return cellBelow;
        }
        return this;
    }

}
