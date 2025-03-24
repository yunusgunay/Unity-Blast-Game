using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public static class RocketCombo
{
    public static async void ExecuteCombo(Cell tappedCell)
    {
        // Ensure asynchronous execution.
        await Task.Yield();

        GameBoard board = RocketManager.Instance.board;
        int centerX = tappedCell.X;
        int centerY = tappedCell.Y;

        // 1) Gather tapped rocket and one adjacent rocket (if any)
        Cell neighborRocketCell = null;
        List<Cell> rocketCells = new List<Cell>();
        rocketCells.Add(tappedCell);
        foreach (Cell neighbor in tappedCell.neighbours)
        {
            if (neighbor.item is RocketItem)
            {
                neighborRocketCell = neighbor;
                rocketCells.Add(neighbor);
                break; // One neighbor is sufficient for combo logic.
            }
        }

        // 2) Remove both rockets from the board.
        foreach (Cell rocketCell in rocketCells)
        {
            if (rocketCell.item != null)
            {
                rocketCell.item.RemoveItem(); // Destroy rocket GameObject.
                rocketCell.item = null;
            }
        }

        // 3) Determine a clamped union area so that we always clear 3 rows and 3 columns (if possible).
        int unionRowStart = centerY - 1;
        if (unionRowStart < 0) unionRowStart = 0;
        if (unionRowStart > board.Rows - 3) unionRowStart = board.Rows - 3;
        int unionRowEnd = unionRowStart + 2;

        int unionColStart = centerX - 1;
        if (unionColStart < 0) unionColStart = 0;
        if (unionColStart > board.Cols - 3) unionColStart = board.Cols - 3;
        int unionColEnd = unionColStart + 2;

        // 4) Clear all items in the union area.
        ClearUnionArea(unionRowStart, unionRowEnd, unionColStart, unionColEnd, board);

        // 5) Spawn parted rockets from every cell on each edge of the union.
        SpawnEdgePartedRockets(unionRowStart, unionRowEnd, unionColStart, unionColEnd, board);
    }

    // Clears all items in:
    // - All cells in rows unionRowStart to unionRowEnd (for every column)
    // - All cells in columns unionColStart to unionColEnd (for every row)
    private static void ClearUnionArea(int unionRowStart, int unionRowEnd, int unionColStart, int unionColEnd, GameBoard board)
    {
        HashSet<Cell> cellsToClear = new HashSet<Cell>();

        // Add every cell in the union rows.
        for (int r = unionRowStart; r <= unionRowEnd; r++)
        {
            for (int c = 0; c < board.Cols; c++)
            {
                cellsToClear.Add(board.Cells[c, r]);
            }
        }
        // Add every cell in the union columns.
        for (int c = unionColStart; c <= unionColEnd; c++)
        {
            for (int r = 0; r < board.Rows; r++)
            {
                cellsToClear.Add(board.Cells[c, r]);
            }
        }

        foreach (Cell cell in cellsToClear)
        {
            if (cell != null && cell.item != null)
            {
                cell.item.TryExecute();
            }
        }
    }

    // Spawns parted rockets from every cell on the edges of the union.
    private static void SpawnEdgePartedRockets(int unionRowStart, int unionRowEnd, int unionColStart, int unionColEnd, GameBoard board)
    {
        // Top edge: row = unionRowStart, for columns unionColStart to unionColEnd.
        for (int col = unionColStart; col <= unionColEnd; col++)
        {
            Cell cell = board.Cells[col, unionRowStart];
            CreatePartedRocket(cell, Direction.Up, board);
        }
        // Bottom edge: row = unionRowEnd.
        for (int col = unionColStart; col <= unionColEnd; col++)
        {
            Cell cell = board.Cells[col, unionRowEnd];
            CreatePartedRocket(cell, Direction.Down, board);
        }
        // Left edge: column = unionColStart.
        for (int row = unionRowStart; row <= unionRowEnd; row++)
        {
            Cell cell = board.Cells[unionColStart, row];
            CreatePartedRocket(cell, Direction.Left, board);
        }
        // Right edge: column = unionColEnd.
        for (int row = unionRowStart; row <= unionRowEnd; row++)
        {
            Cell cell = board.Cells[unionColEnd, row];
            CreatePartedRocket(cell, Direction.Right, board);
        }
    }

    // Instantiates a parted rocket from the given cell in the specified direction.
    private static void CreatePartedRocket(Cell originCell, Direction dir, GameBoard board)
    {
        GameObject partedPrefab = null;
        switch (dir)
        {
            case Direction.Left:
                partedPrefab = RocketManager.Instance.partedLeftPrefab;
                break;
            case Direction.Right:
                partedPrefab = RocketManager.Instance.partedRightPrefab;
                break;
            case Direction.Up:
                partedPrefab = RocketManager.Instance.partedTopPrefab;
                break;
            case Direction.Down:
                partedPrefab = RocketManager.Instance.partedBottomPrefab;
                break;
        }
        if (partedPrefab == null)
        {
            // Fallback: if no prefab is assigned, do line-based damage.
            DamageCellsInDirection(originCell, dir);
            return;
        }
        var partedObj = Object.Instantiate(partedPrefab, board.particlesParent);
        partedObj.transform.position = originCell.transform.position;
        var parted = partedObj.GetComponent<PartedRocket>();
        parted.Init(originCell, dir);
    }

    private static void DamageCellsInDirection(Cell startCell, Direction dir)
    {
        var currentCell = startCell;
        while (true)
        {
            var nextCell = currentCell.GetNeighbourWithDirection(dir);
            if (nextCell == null) break;
            if (nextCell.item != null)
                nextCell.item.TryExecute();
            currentCell = nextCell;
        }
    }
}
