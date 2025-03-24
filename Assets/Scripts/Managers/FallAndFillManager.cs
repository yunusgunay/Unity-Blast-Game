using System.Collections.Generic;
using UnityEngine;

public class FallAndFillManager : Singleton<FallAndFillManager>
{
    private bool isActive;
    private GameBoard board;
    public LevelData levelData;
    private Cell[] fillingCells;

    public void Init(GameBoard board, LevelData levelData)
    {
        this.board = board;
        this.levelData = levelData;
        
        FindFillingCells();
        StartFall();
    }

    public void FindFillingCells()
    {
        var cellList = new List<Cell>();

        for (int y = 0; y < board.Rows; y++)
        {
            for (int x = 0; x < board.Cols; x++)
            {
                var cell = board.Cells[x, y];

                if (cell != null && cell.isFillingCell)
                    cellList.Add(cell);
            }
        }
        fillingCells = cellList.ToArray();
    }

    public void DoFalls()
    {
        // Iterate over all cells and let items fall if there is an empty cell below.
        for (int y = 0; y < board.Rows; y++)
        {
            for (int x = 0; x < board.Cols; x++)
            {
                var cell = board.Cells[x, y];

                if (cell.item != null && cell.firstCellBelow != null && cell.firstCellBelow.item == null)
                {
                    cell.item.Fall();
                }
            }
        }
    }

    public void DoFills()
    {
        // For every "filling" cell (usually the top row cells), if empty, instantiate a new item.
        for (int i = 0; i < fillingCells.Length; i++)
        {
            var cell = fillingCells[i];

            if (cell.item == null)
            {
                // Create a new random cube item (from your LevelData helper)
                cell.item = ItemFactory.Instance.CreateItem(LevelData.GetRandomCubeItemType(), board.itemsParent);

                // Calculate an initial spawn position (above the cell)
                float offsetY = 0.0f;
                var targetCellBelow = cell.GetFallTarget().firstCellBelow;
                if (targetCellBelow != null && targetCellBelow.item != null)
                {
                    offsetY = targetCellBelow.item.transform.position.y + 1;
                }

                var pos = cell.transform.position;
                pos.y += 2;
                pos.y = pos.y > offsetY ? pos.y : offsetY;

                if (cell.item == null) continue;

                cell.item.transform.position = pos;
                cell.item.Fall();
            }
        }
    }

    public void StartFall() { isActive = true; }
    public void StopFall() { isActive = false; }

    private void Update()
    {
        if (!isActive) return;

        DoFalls();
        DoFills();
    }
    
    private void OnEnable()
    {
        // Subscribe to the rocket manager event that signals all parted rockets are finished
        if (RocketManager.Instance != null)
            RocketManager.Instance.OnAllPartedRocketsFinished += HandleAllPartedRocketsDone;
    }

    private void OnDisable()
    {
        if (RocketManager.Instance != null)
            RocketManager.Instance.OnAllPartedRocketsFinished -= HandleAllPartedRocketsDone;
    }

    private void HandleAllPartedRocketsDone()
    {
        // When all parted rockets have finished their explosion,
        // resume falling/filling of the board
        StartFall();
    }
}
