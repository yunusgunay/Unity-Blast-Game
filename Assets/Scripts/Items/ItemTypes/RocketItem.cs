using UnityEngine;

public class RocketItem : Item
{
    public RocketDirection rocketDirection;

    public void PrepareRocketItem(ItemBase itemBase, RocketDirection direction)
    {
        rocketDirection = direction;
        itemBase.IsFallable = true;
        itemBase.InterectWithExplode = false;
        itemBase.Clickable = true;

        switch(direction)
        {
            case RocketDirection.Horizontal:
                Prepare(itemBase, ImageConverter.Instance.HorizontalRocket);
                break;
            case RocketDirection.Vertical:
                Prepare(itemBase, ImageConverter.Instance.VerticalRocket);
                break;
        }
    }

    public override MatchType GetMatchType()
    {
        return MatchType.Special;
    }

    public override void TryExecute()
    {
        FallAndFillManager.Instance.StopFall(); // First rocket fly, then cubes fall.
        ExplodeRocket();
        base.TryExecute(); // Removes rocket from the board.
    }

    private void ExplodeRocket()
    {
        ExplodeWithPartedRockets();
    }

    private void ExplodeWithPartedRockets()
    {
        if (rocketDirection == RocketDirection.Horizontal)
        {
            CreatePartedRocket(Direction.Left, RocketManager.Instance.partedLeftPrefab);
            CreatePartedRocket(Direction.Right, RocketManager.Instance.partedRightPrefab);
        }
        else
        {
            CreatePartedRocket(Direction.Up, RocketManager.Instance.partedTopPrefab);
            CreatePartedRocket(Direction.Down, RocketManager.Instance.partedBottomPrefab);
        }
    }

    private void CreatePartedRocket(Direction dir, GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogWarning($"No parted rocket prefab assigned for direction {dir}. Falling back to line-based damage.");
            DamageCellsInDirection(dir);
            return;
        }
        var partedObj = Instantiate(prefab, Cell.gameGrid.particlesParent);
        partedObj.transform.position = Cell.transform.position;
        var parted = partedObj.GetComponent<PartedRocket>();
        parted.Init(Cell, dir);
    }

    private void DamageCellsInDirection(Direction direction)
    {
        var currentCell = Cell;
        while (true)
        {
            var nextCell = currentCell.GetNeighbourWithDirection(direction);
            if (nextCell == null) break;
            if (nextCell.item != null)
            {
                nextCell.item.TryExecute();
            }
            currentCell = nextCell;
        }
    }
}
