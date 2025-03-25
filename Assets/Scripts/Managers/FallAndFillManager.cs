using System.Collections.Generic;
using UnityEngine;

// FallAndFillManager manages the cubes' fall logic in the game.
public class FallAndFillManager : MonoBehaviour {
    public static FallAndFillManager Instance { get; private set; }

    private bool isActive;
    private GameBoard board;
    public LevelData levelData;
    private Cell[] fillingCells;
    public RocketManager rocketManager;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Init(GameBoard board, LevelData levelData) {
        this.board = board;
        this.levelData = levelData;
        FindEmptyCells();
        StartFall();
    }

    public void FindEmptyCells() {
        List<Cell> cellList = new List<Cell>();
    
        for (int y = 0; y < board.boardRows; ++y) {
            for (int x = 0; x < board.boardCols; ++x){
                Cell cell = board.Cells[x, y];

                if (cell != null && cell.isFillingCell) {
                    cellList.Add(cell);
                }
            }
        }
    
        fillingCells = cellList.ToArray();
    }

    public void ExecuteFalls() {
        for (int y = 0; y < board.boardRows; ++y) {
            for (int x = 0; x < board.boardCols; ++x) {
                Cell cell = board.Cells[x, y];

                if (cell.item != null && cell.cellBelow != null && cell.cellBelow.item == null) {
                    cell.item.Fall();
                }
            }
        }
    }

    public void FillEmptyCells() {
        for (int i = 0; i < fillingCells.Length; i++) {
            Cell cell = fillingCells[i];

            if (cell.item == null) {
                // Create a new random cube item
                cell.item = ItemFactory.Instance.CreateItem(LevelData.GetRandomCubeItemType(), board.itemsParent);

                // Calculate an initial spawn position (above the cell)
                float offsetY = 0.0f;
                Cell targetCellBelow = cell.FindFallTarget().cellBelow;
                if (targetCellBelow != null && targetCellBelow.item != null) {
                    offsetY = targetCellBelow.item.transform.position.y + 1;
                }

                var pos = cell.transform.position;
                pos.y += 2;
                pos.y = pos.y > offsetY ? pos.y : offsetY;

                if (cell.item == null) { continue; }

                cell.item.transform.position = pos;
                cell.item.Fall();
            }
        }
    }

    public void StartFall() { isActive = true; }
    public void StopFall() { isActive = false; }

    private void Update() {
        if (!isActive) { return; }

        ExecuteFalls();
        FillEmptyCells();
    }
    
    private void OnEnable() {
        if (rocketManager != null)
            rocketManager.OnAllPartedRocketsFinished += HandleAllPartedRocketsDone;
    }

    private void OnDisable() {
        if (rocketManager != null)
            rocketManager.OnAllPartedRocketsFinished -= HandleAllPartedRocketsDone;
    }

    private void HandleAllPartedRocketsDone() {
        StartFall();
    }

}
