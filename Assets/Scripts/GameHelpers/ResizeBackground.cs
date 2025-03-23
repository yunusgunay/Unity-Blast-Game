using UnityEngine;

public class ResizeBackground : MonoBehaviour
{
    [SerializeField] private GameBoard gameGrid;
    [SerializeField] private SpriteRenderer sr;

    private const float WIDTH_PADDING = 0.45f;
    private const float HEIGHT_PADDING = 0.60f;

    private void Awake()
    {
        if (sr == null)
            sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // Call Resize after GameBoard has finished its Awake method
        Resize();
    }

    public void Resize()
    {
        if (gameGrid == null)
        {
            Debug.LogWarning("GameGrid is null in ResizeBackground.Resize()");
            return;
        }

        // Use the GameBoard's Cols and Rows properties instead of levelInfo directly
        int gridWidth = gameGrid.Cols;
        int gridHeight = gameGrid.Rows;
        
        Debug.Log($"Grid Width: {gridWidth}, Grid Height: {gridHeight}");
        
        float newWidth = gridWidth + WIDTH_PADDING;
        float newHeight = gridHeight + HEIGHT_PADDING;
        
        // Ensure minimum size
        newWidth = Mathf.Max(newWidth, WIDTH_PADDING);
        newHeight = Mathf.Max(newHeight, HEIGHT_PADDING);
        
        Debug.Log($"Resizing background to: {newWidth} x {newHeight}");
        
        sr.size = new Vector2(newWidth, newHeight);
    }
}
