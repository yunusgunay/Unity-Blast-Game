using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/* HintManager handles two types of hints:
    1) BFS-based matching hints for cubes.
    2) Rocket adjacency hints, where any two adjacent rockets continuously shake (DOTween Animation).
*/
public class HintManager : MonoBehaviour {
    [SerializeField] private GameBoard gameBoard;

    private void Update() {
        showHints();
        showRocketAdjacencyHints();
    }

    private void showHints() {
        var visitedCells = new HashSet<Cell>();
        var queue = new Queue<Cell>();

        for (int y = 0; y < gameBoard.boardRows; ++y) {
            for (int x = 0; x < gameBoard.boardCols; ++x) {
                var cell = gameBoard.Cells[x, y];
                if (cell.item == null || visitedCells.Contains(cell)) {
                    continue;
                }

                queue.Enqueue(cell);
                visitedCells.Add(cell);

                while (queue.Count > 0) {
                    var currCell = queue.Dequeue();
                    var matchedCells = MatchingManager.Instance.FindMatches(currCell, currCell.item.GetMatchType());
                    var matchedCubeCount = MatchingManager.Instance.CountMatchedCubeItem(matchedCells);

                    // Mark matched cells as visited
                    foreach (var matchedCell in matchedCells) {
                        if (!visitedCells.Contains(matchedCell)) {
                            visitedCells.Add(matchedCell);
                            queue.Enqueue(matchedCell);
                        }
                    }

                    // Update hints for each matched cell
                    for (int i = 0; i < matchedCubeCount && i < matchedCells.Count; ++i) {
                        var currItem = matchedCells[i].item;
                        SpriteUpdate(currItem, matchedCubeCount);
                    }
                }
            }
        }
    }

    private void SpriteUpdate(Item item, int matchedCount) {
        if (matchedCount >= 4) {
            item.UpdateToHintSprite(ITEM_TYPE.HorizontalRocket);
            return;
        } 
        
        item.UpdateToHintSprite(item.ItemType);
    }

    private HashSet<Item> scaledUpRockets = new HashSet<Item>();
    private Dictionary<Item, Tween> rocketShakeTweens = new Dictionary<Item, Tween>();
    private void showRocketAdjacencyHints() {
        if (gameBoard == null) { return; }

        // This set will store all rockets that have a rocket neighbor
        HashSet<Item> newScaledUp = new HashSet<Item>();

        // Scan the board for rocket items
        for (int y = 0; y < gameBoard.boardRows; y++) {
            for (int x = 0; x < gameBoard.boardCols; x++) {
                Cell cell = gameBoard.Cells[x, y];
                
                if (cell.item is RocketItem rocket) {
                    // Check adjacency
                    bool hasRocketNeighbor = false;
                    foreach (Cell neighbor in cell.adjacentCells) {
                        if (neighbor.item is RocketItem) {
                            hasRocketNeighbor = true;
                            break;
                        }
                    }

                    if (hasRocketNeighbor) {
                        newScaledUp.Add(rocket);
                        // Do scale up + infinite shake (DOTween Animation)
                        if (!scaledUpRockets.Contains(rocket)) {
                            rocket.transform.DOScale(1.3f, 0.2f) // scale up
                                .SetEase(Ease.OutBack)
                                .OnComplete(() =>
                                {
                                    Tween shake = rocket.transform.DOShakePosition(
                                        duration: 0.5f,     
                                        strength: 0.1f,   
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
                    else {
                        // No rocket neighbor => if it was scaled up, revert
                        if (scaledUpRockets.Contains(rocket)) {
                            if (rocketShakeTweens.TryGetValue(rocket, out Tween shakeTween)) {
                                shakeTween.Kill();
                                rocketShakeTweens.Remove(rocket);
                            }
                            rocket.transform.DOScale(1.0f, 0.2f).SetEase(Ease.InBack); // scale down
                        }
                    }
                }
            }
        }

        // Update the set of scaled up rockets
        scaledUpRockets = newScaledUp;
    }

}
