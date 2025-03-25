using UnityEngine;

public class LevelManager : MonoBehaviour {
    [SerializeField] private GameBoard gameBoard;
    [SerializeField] private GoalManager goalManager;
    [SerializeField] private MovesTracker movesManager;
    private LevelGridData levelData;

    private void Start() {
        if (!CheckRequiredComponents()) { return; }

        InitLevel();
        InitCubesFall();
        
        movesManager.Init(levelData.Moves);
        goalManager.InitGoal(levelData.Goals);
    }

    private bool CheckRequiredComponents() {
        if (gameBoard == null) {
            Debug.LogError("GameBoard is not assigned to LevelManager.");
            return false;
        }

        if (goalManager == null) {
            Debug.LogError("GoalManager is not assigned to LevelManager.");
            return false;
        }

        if (movesManager == null) {
            Debug.LogError("MovesTracker is not assigned to LevelManager.");
            return false;
        }

        return true;
    }

    private void InitLevel() {
        levelData = new LevelGridData(gameBoard.levelInfo);

        for (int i = 0; i < gameBoard.levelInfo.grid_height; ++i) {
            for (int j = 0; j < gameBoard.levelInfo.grid_width; ++j) {
                CreateItemForCell(i, j);
            }
        }
    }

    private void CreateItemForCell(int row, int col) {
        Cell currCell = gameBoard.Cells[col, row];
        ITEM_TYPE itemType = levelData.GridData[gameBoard.levelInfo.grid_height - row - 1, col];
        Item item = ItemCreator.Instance.CreateItem(itemType, gameBoard.itemsParent);

        if (item == null) { return; }

        currCell.item = item;
        item.transform.position = currCell.transform.position;
    }

    private void InitCubesFall() {
        FallAndFillManager.Instance.Init(gameBoard, levelData);
    }

    // JSON Level Logic
    public static LevelInterface GetLevelInfo(int level) {
        string path = "Levels/level_" + level.ToString("00");
        TextAsset jsonFile = Resources.Load<TextAsset>(path);

        if (jsonFile == null) {
            Debug.LogError($"Level file not found at path: {path}");
            return null;
        }

        string jsonString = jsonFile.text;
        if (string.IsNullOrEmpty(jsonString)) {
            Debug.LogError($"Level file is empty at path: {path}");
            return null;
        }

        return JsonUtility.FromJson<LevelInterface>(jsonString);
    }

}
