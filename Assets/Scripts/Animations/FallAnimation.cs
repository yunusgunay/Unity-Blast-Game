using UnityEngine;
using DG.Tweening;

public class FallAnimation : MonoBehaviour {
    public Item item;
    private const float fallDuration = 0.35f; // Time to complete the fall tween.
    private bool applyHorizontalOffset = true; // If true, add a slight random X offset.
    private Cell destinationCell;
    
    private void Start()
    {
        DOTween.SetTweensCapacity(500, 50);
    }

    public void TryFall(Cell newCell)
    {
        // 1) Validate the new cell to ensure it's a valid downward move.
        if (ShouldAbortFall(newCell)) return;

        // 2) Assign the item to that new cell and store the cell reference.
        UpdateDestinationCell(newCell);

        // 3) Perform the actual fall animation.
        PerformFallTween();
    }

    private bool ShouldAbortFall(Cell newCell)
    {
        if (destinationCell != null && newCell.Y >= destinationCell.Y)
            return true;
        
        return false;
    }


    private void UpdateDestinationCell(Cell newCell)
    {
        destinationCell = newCell;
        item.Cell = destinationCell;
    }

    /// <summary>
    /// Executes the DOTween animation to move the item to the new cell's position.
    /// Adds a small bounce and optional random X offset for visual variety.
    /// </summary>
    private void PerformFallTween()
    {
        if (destinationCell == null) return;
        
        Vector3 endPos = destinationCell.transform.position;

        // Optional slight horizontal offset to make falls look less uniform.
        if (applyHorizontalOffset)
        {
            float randomOffset = Random.Range(-0.05f, 0.05f);
            endPos.x += randomOffset;
        }

        item.transform.DOMove(endPos, fallDuration)
            .SetEase(Ease.OutQuad) // Adds a small bounce at the end
            .OnComplete(() =>
            {
                destinationCell = null;
            });
    }
}
