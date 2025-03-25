using UnityEngine;

// ItemBase is the base class for all items in the game.
public class ItemBase : MonoBehaviour {
    public ITEM_TYPE Type;
    public bool IsClickable = true;
    public bool IsFallable = true;
    public bool CanExplode = false;
    public int Health = 1;
    public FallAnimation FallAnimation;
}
