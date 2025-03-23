using DG.Tweening;
using UnityEngine;

public class PartedRocket : MonoBehaviour
{
    public float moveSpeed = 10f;
    private Cell currentCell;
    private Direction travelDirection;
    private bool isActive;
    private float reachThreshold = 0.1f; // How close to the target center counts as "reached"

    public void Init(Cell startCell, Direction dir)
    {
        currentCell = startCell;
        travelDirection = dir;
        transform.position = startCell.transform.position;
        isActive = true;
        // Notify RocketManager that a parted rocket was spawned.
        RocketManager.Instance.PartedRocketSpawned();

        transform.DOScale(1.1f, 0.4f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    private void Update()
    {
        if (!isActive) return;

        // Determine the next cell in the travel direction.
        Cell nextCell = currentCell.GetNeighbourWithDirection(travelDirection);
        if (nextCell == null)
        {
            // No further cell: finish the parted rocket.
            isActive = false;
            Destroy(gameObject);
            return;
        }
        
        Vector3 targetPos = nextCell.transform.position;
        // Move towards the target cell center.
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        // When reached close enough, damage that cell's item.
        if (Vector3.Distance(transform.position, targetPos) < reachThreshold)
        {
            if (nextCell.item != null)
            {
                nextCell.item.TryExecute();
            }
            // Update current cell.
            currentCell = nextCell;
        }
    }

    private void OnDestroy()
    {
        if (RocketManager.Instance != null)
            RocketManager.Instance.PartedRocketFinished();
    }
}
