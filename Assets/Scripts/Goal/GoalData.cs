using System;

// Single goal requirement: which item is needed and how many.
[Serializable]
public struct GoalData {
    public ITEM_TYPE GoalObstacle;
    public int RequiredCount;
}
