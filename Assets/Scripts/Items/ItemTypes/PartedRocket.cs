using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PartedRocket : MonoBehaviour
{
    // How long (in seconds) to traverse the entire path
    public float travelTime = 0.3f;
    
    private List<Cell> pathCells = new List<Cell>();
    
    private Vector3[] pathPositions;

    public void Init(Cell startCell, Direction dir)
    {
        // Gather all cells along the chosen direction
        pathCells.Clear();
        pathCells.Add(startCell);
        Cell current = startCell;
        while (true)
        {
            var next = current.GetNeighbourWithDirection(dir);
            if (next == null) break;
            pathCells.Add(next);
            current = next;
        }
        if (pathCells.Count < 2)
        {
            Destroy(gameObject);
            return;
        }
        // Build an array of positions for DOTween to follow
        pathPositions = new Vector3[pathCells.Count];
        for (int i = 0; i < pathCells.Count; i++)
        {
            pathPositions[i] = pathCells[i].transform.position;
        }
        // Place the rocket at the first cell's position
        transform.position = pathPositions[0];
        // Start the DOTween path animation
        transform.DOPath(pathPositions, travelTime, PathType.Linear, PathMode.Full3D)
            .SetEase(Ease.Linear)
            .OnWaypointChange(OnWaypointChange)
            .OnComplete(() => Destroy(gameObject));
        // Notify RocketManager that a parted rocket was spawned
        RocketManager.Instance.PartedRocketSpawned();
    }

    // Called each time a new waypoint (cell) is reached
    private void OnWaypointChange(int waypointIndex)
    {
        if (waypointIndex < 0 || waypointIndex >= pathCells.Count) return;

        Cell cell = pathCells[waypointIndex];
        // If there's an item, trigger its destruction
        if (cell.item != null)
        {
            cell.item.TryExecute();
        }
        // Spawn a star/fog burst effect at this cell
        if (RocketManager.Instance.fogFXPrefab != null)
        {
            Instantiate(RocketManager.Instance.fogFXPrefab,
                        cell.transform.position,
                        Quaternion.identity,
                        cell.gameGrid.particlesParent);
        }
    }

    private void OnDestroy()
    {
        if (RocketManager.Instance != null)
            RocketManager.Instance.PartedRocketFinished();
    }
}
