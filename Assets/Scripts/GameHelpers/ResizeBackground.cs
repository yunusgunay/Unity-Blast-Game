using UnityEngine;

// Resizes the black background of the grid.
public class ResizeBackground : MonoBehaviour {
    [SerializeField] private GameBoard gameBoard;
    [SerializeField] private SpriteRenderer background;

    private void Awake() {
        if (background == null) {
            background = GetComponent<SpriteRenderer>();
        }
    }

    private void Start() {
        Resize();
    }

    public void Resize() {
        if (gameBoard == null) {
            Debug.LogWarning("GameGrid is null in ResizeBackground.Resize()");
            return;
        }

        int gridWidth = gameBoard.boardCols;
        int gridHeight = gameBoard.boardRows;
        
        Debug.Log($"Grid Width: {gridWidth}, Grid Height: {gridHeight}");
        
        float newWidth = gridWidth + 0.45f;
        float newHeight = gridHeight + 0.60f;
             
        Debug.Log($"Resizing background to: {newWidth} x {newHeight}");
        
        background.size = new Vector2(newWidth, newHeight);
    }

}
