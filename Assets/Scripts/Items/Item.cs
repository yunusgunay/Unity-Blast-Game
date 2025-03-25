using UnityEngine;

public class Item : MonoBehaviour {
    private const int SORTING_ORDER = 10;   // constant for sorting layer order offsets
    private static int childSpriteOrder;    // used to track how many child sprites have been created so far
    
    public SpriteRenderer SpriteRenderer;
    public FallAnimation FallAnimation;
    
    public ITEM_TYPE ItemType;
    [HideInInspector] public bool IsClickable, IsFallable, CanExplode;
    [HideInInspector] public int Health;

    private Cell itemCell;
    public Cell Cell {
        get { return itemCell; }
        set {
            if (itemCell == value) {
                return;
            } 
            else {
                Cell oldCell = itemCell;
                itemCell = value;

                if (oldCell != null && oldCell.item == this) {
                    oldCell.item = null;
                } else {
                    // do nothing
                }

                if (itemCell != null) {
                    itemCell.item = this;
                    gameObject.name = itemCell.gameObject.name + " " + GetType().Name;
                } else {
                    // cell is null, no linking needed
                }
            }
        }
    }

    // Sets up the item by copying properties from the provided itemBase and creating a sprite.
    public void PrepareItem(ItemBase itemBase, Sprite sprite) {
        SpriteRenderer = AddSprite(sprite);

        ItemType = itemBase.Type;
        IsClickable = itemBase.IsClickable;
        IsFallable = itemBase.IsFallable;
        CanExplode = itemBase.CanExplode;
        Health = itemBase.Health;
        FallAnimation = itemBase.FallAnimation;

        if (FallAnimation != null) {
            FallAnimation.item = this;
        }
    }

    // Creates a new child object with its own SpriteRenderer and basic properties set.
    public SpriteRenderer AddSprite(Sprite sprite) {
        GameObject childObj = new GameObject("Sprite_" + childSpriteOrder);
        SpriteRenderer newSpriteRenderer = childObj.AddComponent<SpriteRenderer>();

        childObj.transform.SetParent(transform);
        childObj.transform.localPosition = Vector3.zero;
        childObj.transform.localScale = new Vector2(0.7f, 0.7f);

        newSpriteRenderer.sprite = sprite;
        newSpriteRenderer.sortingLayerID = SortingLayer.NameToID("Cell");
        newSpriteRenderer.sortingOrder = SORTING_ORDER + childSpriteOrder++;

        return newSpriteRenderer;
    }

    public virtual MATCH_TYPE GetMatchType() {
        return MATCH_TYPE.None; // default
    }

    public virtual void TryExecute() {
        GoalManager.Instance.UpdateLevelGoal(ItemType);
        RemoveItem();
    }

    // Removes the item from its cell and destroys the GameObject.
    public void RemoveItem() {
        if (Cell != null) {
            Cell.item = null;
        }
        Cell = null;
        Destroy(gameObject);
    }

    // Changes the item’s active sprite at runtime.
    public void UpdateSprite(Sprite sprite) {
        SpriteRenderer childRenderer = GetComponentInChildren<SpriteRenderer>();
        if (childRenderer != null) {
            childRenderer.sprite = sprite;
        }
    }

    // Override this method to provide hint behavior if needed.
    public virtual void UpdateToHintSprite(ITEM_TYPE itemType) {
        return;
    }

    public void Fall() {
        if (!IsFallable) {
            return;
        } else {
            Cell fallTarget = itemCell.FindFallTarget();
            FallAnimation.TryFall(fallTarget);
        }
    }

}
