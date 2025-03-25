using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// This class represents ROCKET PARTS of the exploided rockets.
public class PartedRocket : MonoBehaviour {
    public float travelTime = 0.3f;
    private List<Cell> pathCells = new List<Cell>();
    private Vector3[] pathPositions;

    public void Init(Cell startCell, DIRECTION dir) {
        pathCells.Clear();
        pathCells.Add(startCell);
        
        Cell current = startCell;
        while (true) {
            var next = current.GetNeighbor(dir);
            if (next == null) { break; }
            pathCells.Add(next);
            current = next;
        }

        if (pathCells.Count < 2) {
            Destroy(gameObject);
            return;
        }

        pathPositions = new Vector3[pathCells.Count]; // an array of positions for DOTween to follow
        for (int i = 0; i < pathCells.Count; ++i) {
            pathPositions[i] = pathCells[i].transform.position;
        }

        transform.position = pathPositions[0]; // place the rocket
        transform.DOPath(pathPositions, travelTime, PathType.Linear, PathMode.Full3D) // DOTween animation
            .SetEase(Ease.Linear)
            .OnWaypointChange(OnWaypointChange)
            .OnComplete(() => Destroy(gameObject));

        RocketManager.Instance.PartedRocketSpawned(); // notify RocketManager
    }

    // This method is called when each time a new cell is reached.
    private void OnWaypointChange(int waypointIndex) {
        if (waypointIndex < 0 || waypointIndex >= pathCells.Count) { return; }

        Cell cell = pathCells[waypointIndex];
        if (cell.item != null) {
            cell.item.TryExecute();
        }

        // Spawn smoke effect at this cell.
        if (RocketManager.Instance.smokePrefab != null) {
            Instantiate(RocketManager.Instance.smokePrefab,
                        cell.transform.position,
                        Quaternion.identity,
                        cell.parentBoard.particlesParent);
        }
    }

    private void OnDestroy() {
        if (RocketManager.Instance != null) {
            RocketManager.Instance.PartedRocketFinished();
        }
    }

}
