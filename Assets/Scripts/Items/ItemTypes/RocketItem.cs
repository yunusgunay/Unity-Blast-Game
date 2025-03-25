using UnityEngine;

// This class represents a single ROCKET in a Cell.
public class RocketItem : Item {
    public ROCKET_DIRECTION rocketDirection;

    public void PrepareRocketItem(ItemBase itemBase, ROCKET_DIRECTION direction) {
        rocketDirection = direction;
        itemBase.IsFallable = true;
        itemBase.IsClickable = true;
        itemBase.CanExplode = false;

        switch (direction) {
            case ROCKET_DIRECTION.Horizontal:
                Prepare(itemBase, ImageConverter.Instance.HorizontalRocket);
                break;
            case ROCKET_DIRECTION.Vertical:
                Prepare(itemBase, ImageConverter.Instance.VerticalRocket);
                break;
        }
    }

    public override MATCH_TYPE GetMatchType() {
        return MATCH_TYPE.Special;
    }

    public override void TryExecute() {
        FallAndFillManager.Instance.StopFall(); // first rocket fly, then cubes fall
        ExplodeRocket();
        base.TryExecute(); // remove rocket from the board
    }

    private void ExplodeRocket() {
        ExplodeWithPartedRockets();
    }

    private void ExplodeWithPartedRockets() {
        if (rocketDirection == ROCKET_DIRECTION.Horizontal) {
            CreatePartedRocket(DIRECTION.Left, RocketManager.Instance.partedLeftPrefab);
            CreatePartedRocket(DIRECTION.Right, RocketManager.Instance.partedRightPrefab);
        } else {
            CreatePartedRocket(DIRECTION.Up, RocketManager.Instance.partedTopPrefab);
            CreatePartedRocket(DIRECTION.Down, RocketManager.Instance.partedBottomPrefab);
        }
    }

    private void CreatePartedRocket(DIRECTION dir, GameObject prefab) {
        if (prefab == null) {
            Debug.LogWarning($"No parted rocket prefab assigned for direction {dir}. Falling back to line-based damage.");
            DamageCellsInDirection(dir);
            return;
        }
        
        // Instantiate the parted rocket using the board's particles container.
        var partedObj = Instantiate(prefab, Cell.parentBoard.particlesParent);
        partedObj.transform.position = Cell.transform.position;
        var parted = partedObj.GetComponent<PartedRocket>();
        parted.Init(Cell, dir);
    }

    private void DamageCellsInDirection(DIRECTION direction) {
        Cell currentCell = Cell;
        while (true) {
            Cell nextCell = currentCell.GetNeighbor(direction);
            if (nextCell == null) { break; }
            if (nextCell.item != null) {
                nextCell.item.TryExecute();
            }
            currentCell = nextCell;
        }
    }

}
