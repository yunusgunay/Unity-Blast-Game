using UnityEngine;
using DG.Tweening;

// FallAnimation manages cubes' fall animation to empty/destroyed cells.
public class FallAnimation : MonoBehaviour {
    [HideInInspector] public Item item;
    private const float FALL_DURATION = 0.30f;
    private Cell destinationCell;
    
    private void Start() {
        DOTween.SetTweensCapacity(500, 50);
    }

    public void TryFall(Cell newCell) {
        if (destinationCell != null && newCell.gridY >= destinationCell.gridY) 
            return;
        
        destinationCell = newCell; // set destination cell
        item.Cell = destinationCell;

        PerformFallTween();
    }

    // DOTween animation to fall the cubes and add a small bounce at the end.
    private void PerformFallTween() {
        if (destinationCell == null) { return; }
        
        Vector3 endPos = destinationCell.transform.position;

        float randomOffset = Random.Range(-0.05f, 0.05f);
        endPos.x += randomOffset;

        item.transform.DOMove(endPos, FALL_DURATION)
            .SetEase(Ease.OutQuad) // small bounce at the end
            .OnComplete(() =>
            {
                destinationCell = null;
            });
    }

}
