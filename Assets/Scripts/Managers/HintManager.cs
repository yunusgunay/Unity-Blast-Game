using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// HintManager handles two types of hints:
/// 1) BFS-based matching hints for cubes.
/// 2) Rocket adjacency hints, where any two adjacent rockets continuously shake.
/// </summary>
public class HintManager : MonoBehaviour
{
    [SerializeField] private GameBoard board;

    // For BFS-based matching logic.
    private void Update()
    {
        showHints();
        showRocketAdjacencyHints();
    }

    // ---------------------------------------------------------------------
    // PART A: Your existing BFS-based matching hints for cubes
    // ---------------------------------------------------------------------
    private void showHints()
    {
        var visitedCells = new HashSet<Cell>();
        var queue = new Queue<Cell>();

        for (int y = 0; y < board.Rows; ++y)
        {
            for (int x = 0; x < board.Cols; ++x)
            {
                var cell = board.Cells[x, y];
                if (cell.item == null || visitedCells.Contains(cell))
                    continue;

                queue.Enqueue(cell);
                visitedCells.Add(cell);

                while (queue.Count > 0)
                {
                    var currCell = queue.Dequeue();
                    var matchedCells = MatchingManager.Instance.FindMatches(currCell, currCell.item.GetMatchType());
                    var matchedCubeCount = MatchingManager.Instance.CountMatchedCubeItem(matchedCells);

                    // Mark matched cells as visited
                    foreach (var matchedCell in matchedCells)
                    {
                        if (!visitedCells.Contains(matchedCell))
                        {
                            visitedCells.Add(matchedCell);
                            queue.Enqueue(matchedCell);
                        }
                    }

                    // Update hints for each matched cell
                    for (int i = 0; i < matchedCubeCount && i < matchedCells.Count; ++i)
                    {
                        var currItem = matchedCells[i].item;
                        spriteUpdate(currItem, matchedCubeCount);
                    }
                }
            }
        }
    }

    private void spriteUpdate(Item item, int matchedCount)
    {
        if (matchedCount >= 4)
        {
            item.HintUpdateToSprite(ItemType.HorizontalRocket);
        }
        else
        {
            item.HintUpdateToSprite(item.ItemType);
        }
    }

    // ---------------------------------------------------------------------
    // PART B: Rocket adjacency + continuous shaking
    // ---------------------------------------------------------------------

    // We store which rockets are "active" in adjacency (scaled up)...
    private HashSet<Item> scaledUpRockets = new HashSet<Item>();
    // ...and also a dictionary from rocket => its shake tween, so we can kill it.
    private Dictionary<Item, Tween> rocketShakeTweens = new Dictionary<Item, Tween>();

    private void showRocketAdjacencyHints()
    {
        if (board == null) return;

        // This set will store all rockets that have a rocket neighbor (and thus remain scaled/shaking)
        HashSet<Item> newScaledUp = new HashSet<Item>();

        // Scan the board for rocket items
        for (int y = 0; y < board.Rows; y++)
        {
            for (int x = 0; x < board.Cols; x++)
            {
                Cell cell = board.Cells[x, y];
                if (cell.item is RocketItem rocket)
                {
                    // Check adjacency
                    bool hasRocketNeighbor = false;
                    foreach (Cell neighbor in cell.neighbours)
                    {
                        if (neighbor.item is RocketItem)
                        {
                            hasRocketNeighbor = true;
                            break;
                        }
                    }

                    if (hasRocketNeighbor)
                    {
                        // This rocket should be scaled + shaking
                        newScaledUp.Add(rocket);

                        // If it's newly discovered, do scale up + infinite shake
                        if (!scaledUpRockets.Contains(rocket))
                        {
                            // Scale up
                            rocket.transform.DOScale(1.3f, 0.2f)
                                .SetEase(Ease.OutBack)
                                .OnComplete(() =>
                                {
                                    // Once scale-up finishes, start continuous shaking
                                    Tween shake = rocket.transform.DOShakePosition(
                                        duration: 0.5f,     // how long one shake cycle lasts
                                        strength: 0.1f,     // offset
                                        vibrato: 10,
                                        randomness: 90,
                                        snapping: false,
                                        fadeOut: false
                                    )
                                    .SetLoops(-1, LoopType.Restart); // infinite loop

                                    rocketShakeTweens[rocket] = shake;
                                });
                        }
                    }
                    else
                    {
                        // No rocket neighbor => if it was scaled up, revert
                        if (scaledUpRockets.Contains(rocket))
                        {
                            // Kill the shake tween if it exists
                            if (rocketShakeTweens.TryGetValue(rocket, out Tween shakeTween))
                            {
                                shakeTween.Kill();
                                rocketShakeTweens.Remove(rocket);
                            }
                            // Scale down
                            rocket.transform.DOScale(1.0f, 0.2f).SetEase(Ease.InBack);
                        }
                    }
                }
            }
        }

        // Update the set of scaled up rockets
        scaledUpRockets = newScaledUp;
    }
}
