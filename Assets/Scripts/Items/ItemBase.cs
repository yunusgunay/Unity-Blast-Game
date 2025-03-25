using UnityEngine;

// ItemBase is the base class for all items in the game.
public class ItemBase : MonoBehaviour {
    [HideInInspector] public ITEM_TYPE Type = ITEM_TYPE.None;
    [HideInInspector] public bool IsClickable = true;
    [HideInInspector] public bool IsFallable = true;
    [HideInInspector] public bool CanExplode = false;
    [HideInInspector] public int Health = 1;
    public FallAnimation FallAnimation;
}
