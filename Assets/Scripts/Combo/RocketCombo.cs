using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

// Rocket Combo system of the game.
public static class RocketCombo {
    public static async void ExecuteCombo(Cell tappedCell) {
        await Task.Yield();

        GameBoard board = tappedCell.parentBoard;
        int centerX = tappedCell.gridX;
        int centerY = tappedCell.gridY;

        // 1. Gather tapped rocket and, if any, one adjacent rocket.
        List<Cell> rocketCells = new List<Cell> { tappedCell };
        foreach (Cell neighbor in tappedCell.adjacentCells) {
            if (neighbor.item is RocketItem) {
                rocketCells.Add(neighbor);
                break; // one neighbor is sufficient
            }
        }

        // 2. Remove both rockets from the board.
        foreach (Cell rocketCell in rocketCells) {
            if (rocketCell.item != null) {
                rocketCell.item.RemoveItem();
                rocketCell.item = null;
            }
        }

        // 3. Determine the union area to clear 3 rows and 3 columns.
        int unionRowStart = centerY - 1;
        if (unionRowStart < 0) unionRowStart = 0;
        if (unionRowStart > board.boardRows - 3) unionRowStart = board.boardRows - 3;
        int unionRowEnd = unionRowStart + 2;

        int unionColStart = centerX - 1;
        if (unionColStart < 0) unionColStart = 0;
        if (unionColStart > board.boardCols - 3) unionColStart = board.boardCols - 3;
        int unionColEnd = unionColStart + 2;

        // 4. Clear all items in the union area and spawn parted rockets.
        ClearUnionArea(unionRowStart, unionRowEnd, unionColStart, unionColEnd, board);
        SpawnEdgePartedRockets(unionRowStart, unionRowEnd, unionColStart, unionColEnd, board);
    }

    private static void ClearUnionArea(int unionRowStart, int unionRowEnd, int unionColStart, int unionColEnd, GameBoard board) {
        HashSet<Cell> cellsToClear = new HashSet<Cell>();
        for (int row = unionRowStart; row <= unionRowEnd; row++) {
            for (int col = 0; col < board.boardCols; col++) {
                cellsToClear.Add(board.Cells[col, row]);
            }
        }
        for (int col = unionColStart; col <= unionColEnd; col++) {
            for (int row = 0; row < board.boardRows; row++) {
                cellsToClear.Add(board.Cells[col, row]);
            }
        }
    
        foreach (Cell cell in cellsToClear) {
            if (cell != null && cell.item != null) {
                cell.item.TryExecute();
            }
        }
    }

    private static void SpawnEdgePartedRockets(int unionRowStart, int unionRowEnd, int unionColStart, int unionColEnd, GameBoard board) {
        // Top Edge
        for (int col = unionColStart; col <= unionColEnd; ++col) {
            Cell cell = board.Cells[col, unionRowStart];
            CreatePartedRocket(cell, DIRECTION.Up, board);
        }
        // Bottom Edge
        for (int col = unionColStart; col <= unionColEnd; ++col) {
            Cell cell = board.Cells[col, unionRowEnd];
            CreatePartedRocket(cell, DIRECTION.Down, board);
        }
        // Left Edge
        for (int row = unionRowStart; row <= unionRowEnd; ++row) {
            Cell cell = board.Cells[unionColStart, row];
            CreatePartedRocket(cell, DIRECTION.Left, board);
        }
        // Right Edge
        for (int row = unionRowStart; row <= unionRowEnd; ++row) {
            Cell cell = board.Cells[unionColEnd, row];
            CreatePartedRocket(cell, DIRECTION.Right, board);
        }
    }

    private static void CreatePartedRocket(Cell originCell, DIRECTION dir, GameBoard board) {
        GameObject partedPrefab = null;
        switch (dir) {
            case DIRECTION.Left:
                partedPrefab = RocketManager.Instance.partedLeftPrefab;
                break;
            case DIRECTION.Right:
                partedPrefab = RocketManager.Instance.partedRightPrefab;
                break;
            case DIRECTION.Up:
                partedPrefab = RocketManager.Instance.partedTopPrefab;
                break;
            case DIRECTION.Down:
                partedPrefab = RocketManager.Instance.partedBottomPrefab;
                break;
        }

        if (partedPrefab == null) {
            DamageCellsInDirection(originCell, dir);
            return;
        }

        var partedObj = Object.Instantiate(partedPrefab, board.particlesParent); // instantiate the prefab
        partedObj.transform.position = originCell.transform.position; // set its position
        var parted = partedObj.GetComponent<PartedRocket>(); // get PartedRocket component
        parted.Init(originCell, dir); // initialize the PartedRocket
    }

    private static void DamageCellsInDirection(Cell startCell, DIRECTION dir) {
        Cell currCell = startCell;
        while (true) {
            Cell nextCell = currCell.GetNeighbor(dir);
            if (nextCell == null) { break; }
            if (nextCell.item != null) {
                nextCell.item.TryExecute();
            }
            currCell = nextCell;
        }
    }

}
