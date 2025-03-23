using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PartedRocket : MonoBehaviour
{
    // How long (in seconds) it takes to travel from the first cell to the last cell
    public float travelTime = 0.5f;

    private List<Cell> pathCells = new List<Cell>();

    public void Init(Cell startCell, Direction dir)
    {
        // 1) Gather all cells in the chosen direction
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

        // If there's only 1 cell (the startCell), nothing to travel
        if (pathCells.Count <= 1)
        {
            Destroy(gameObject);
            return;
        }

        // 2) Build an array of positions for DOTween to follow
        Vector3[] positions = new Vector3[pathCells.Count];
        for (int i = 0; i < pathCells.Count; i++)
        {
            positions[i] = pathCells[i].transform.position;
        }

        // 3) Move the parted rocket along these positions
        //    The rocket will begin at the first cell, so place it there initially
        transform.position = positions[0];

        // 4) Create the path tween
        transform.DOPath(positions, travelTime, PathType.Linear, PathMode.Full3D)
            .SetEase(Ease.Linear)
            // OnWaypointChange is called each time we reach a new position in 'positions'
            .OnWaypointChange(OnWaypointChange)
            // When done traveling, destroy parted rocket
            .OnComplete(() => Destroy(gameObject));

        // 5) Notify rocket manager we spawned a parted rocket
        RocketManager.Instance.PartedRocketSpawned();
    }

    // Called each time we pass or arrive at a new waypoint (cell)
    private void OnWaypointChange(int waypointIndex)
    {
        if (waypointIndex < 0 || waypointIndex >= pathCells.Count) return;

        // Attempt to destroy the item in that cell
        Cell cell = pathCells[waypointIndex];
        if (cell.item != null)
        {
            cell.item.TryExecute();
        }
    }

    private void OnDestroy()
    {
        if (RocketManager.Instance != null)
        {
            RocketManager.Instance.PartedRocketFinished();
        }
    }
}
