using System.Collections.Generic;
using UnityEngine;

/* GroupChecker finds and explodes groups of matched cells. 
It also spawns rocket items if the match is large enough. */
public class GroupChecker : Singleton<GroupChecker> {
    [SerializeField] private GameBoard board;
    private bool[,] visited; // 2D array
    private const int MIN_SIZE = 2;

    public void Start() {
        visited = new bool[board.boardCols, board.boardRows];
    }

    void ResetVisited() {
        for (int i = 0; i < visited.GetLength(0); ++i) {
            for (int j = 0; j < visited.GetLength(1); ++j) {
                visited[i, j] = false;
            }
        }
    }

    public List<Cell> FindMatches(Cell startCell, MATCH_TYPE matchType) {
        List<Cell> result = new List<Cell>();
        if (startCell == null) { return result; }
        ResetVisited();

        Queue<Cell> q = new Queue<Cell>();
        int sx = startCell.gridX, sy = startCell.gridY;
        visited[sx, sy] = true;

        if (startCell.item == null || startCell.item.GetMatchType() != matchType || matchType == MATCH_TYPE.None) {
            return result;
        }

        q.Enqueue(startCell);

        // BFS
        while (q.Count > 0) {
            Cell curr = q.Dequeue();
            result.Add(curr);

            if (!curr.item.Clickable) { continue; }

            for (int i = 0; i < curr.adjacentCells.Count; ++i) {
                Cell neighbor = curr.adjacentCells[i];
                int nx = neighbor.gridX, ny = neighbor.gridY;
                if (!visited[nx, ny] && neighbor.item != null && neighbor.item.GetMatchType() == matchType) {
                    visited[nx, ny] = true;
                    q.Enqueue(neighbor);
                }
            }
        }

        return result;
    }

    public int CountGroupCubes(List<Cell> group) {
        int count = 0;
        for (int i = 0; i < group.Count; ++i) {
            if (group[i].item.Clickable) { // only cubes are clickable
                count++;
            }
        }
        return count;
    }

    public void ExplodeMatchingCells(Cell cell) {
        if (cell == null || cell.item == null) { return; }

        List<Cell> group = FindMatches(cell, cell.item.GetMatchType());
        int cubes = CountGroupCubes(group);
        if (cubes < MIN_SIZE) { return; }

        // Keep track of processed neighbors to avoid double-damaging them.
        List<Cell> processed = new List<Cell>();
        for (int i = 0; i < group.Count; ++i) {
            Cell c = group[i];
            BlastNeighbors(c, processed);
            if (c.item != null) {
                c.item.TryExecute();
            }
        }

        _ = MovesTracker.Instance.DecrementMoves();
        SpawnRocketsIfNeeded(cell, cubes);
    }

    void BlastNeighbors(Cell exploded, List<Cell> used) {
        for (int i = 0; i < exploded.adjacentCells.Count; ++i) {
            Cell nb = exploded.adjacentCells[i];
            if (nb.item != null && !used.Contains(nb)) {
                used.Add(nb);
                if (nb.item.InterectWithExplode == true) { nb.item.TryExecute(); }
            }
        }
    }

    private void SpawnRocketsIfNeeded(Cell cell, int matchedCount) {
        // If 4+ cubes were matched, spawn rocket item.
        if (matchedCount < 4) { return; }
        
        ROCKET_DIRECTION randomDir = (Random.Range(0, 2) == 0) ? ROCKET_DIRECTION.Horizontal : ROCKET_DIRECTION.Vertical;
        ITEM_TYPE rocketType = (randomDir == ROCKET_DIRECTION.Horizontal) ? ITEM_TYPE.HorizontalRocket : ITEM_TYPE.VerticalRocket;
        
        cell.item = ItemFactory.Instance.CreateItem(rocketType, board.itemsParent);
        if (cell.item != null) {
            cell.item.transform.position = cell.transform.position;
        }
    }

}
