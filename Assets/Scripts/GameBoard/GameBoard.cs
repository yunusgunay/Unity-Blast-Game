using UnityEngine;

// GameBoard manages the grid of cells in the game.
public class GameBoard : MonoBehaviour {
    public Transform cellsParent, itemsParent, particlesParent;
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private ComboManager comboManager;

    public LevelInterface levelInfo;

    public int boardRows { get; private set; }
    public int boardCols { get; private set; }
    public Cell[,] Cells { get; private set; } // 2D array

    private void Awake() {
        RetrieveLevelData();
        
        Cells = new Cell[boardCols, boardRows];
        ResizeGameBoard(boardRows, boardCols);
        
        CreateCells();      
        InitCells();
    }

    // Loads level data and sets board size.
    private void RetrieveLevelData() {
        int currentLevel = PlayerPrefs.GetInt("Level", 1);
        levelInfo = LevelManager.GetLevelInfo(currentLevel);

        boardRows = levelInfo.grid_height;
        boardCols = levelInfo.grid_width;
    }

    // Instantiates all cells and assigns them to the Cells array.
    private void CreateCells() {
        float spacing = 1f; // assuming each cell sprite is about 1 unit in size

        for (int y = 0; y < boardRows; ++y) {
            for (int x = 0; x < boardCols; ++x) {
                Vector3 position = new Vector3(x * spacing, y * spacing, 0);
                Cell newCell = Instantiate(cellPrefab, position, Quaternion.identity, cellsParent);

                newCell.SetComboManager(comboManager);
                
                Cells[x, y] = newCell;
            }
        }
    }

    private void InitCells() {
        for (int y = 0; y < boardRows; ++y) {
            for (int x= 0; x < boardCols; ++x) {
                Cells[x, y].InitializeCell(x, y, this);
            }
        }
    }

    private void ResizeGameBoard(int rows, int cols) {
        float newX = (9 - cols) * 0.5f;
        float newY = (9 - rows) * 0.5f;
        transform.position = new Vector3(newX, newY, transform.position.z);
    }

}
