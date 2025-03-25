using System;
using System.Collections.Generic;
using UnityEngine;

// GoalManager manages the goal system in a level.
public class GoalManager : Singleton<GoalManager> {
    [SerializeField] private GoalObject goalPrefab;
    [SerializeField] private Transform goalsParent;
    private List<GoalObject> goalObjects = new List<GoalObject>();

    private Dictionary<ITEM_TYPE, int> remainingCounts = new Dictionary<ITEM_TYPE, int>();

    public Action OnGoalsCompleted;
    private bool allGoalsCompleted = false;
    
    public void InitGoal(List<GoalData> goals) {
        goalObjects.Clear();
        remainingCounts.Clear();

        foreach (GoalData goal in goals) {
            GoalObject gObj = Instantiate(goalPrefab, goalsParent);
            gObj.PrepareGoalObj(goal);
            goalObjects.Add(gObj);
            remainingCounts[goal.GoalObstacle] = goal.RequiredCount;
        }
    }

    public void UpdateLevelGoal(ITEM_TYPE itemType) {
        if (allGoalsCompleted) { return; }

        if (remainingCounts.ContainsKey(itemType)) {
            remainingCounts[itemType]--;
            GoalObject gObj = goalObjects.Find(g => g.GetData().GoalObstacle.Equals(itemType));

            if (gObj != null) {
                if (remainingCounts[itemType] <= 0) {
                    remainingCounts[itemType] = 0;
                    gObj.CountLabel.gameObject.SetActive(false);
                    gObj.CompletionMark.gameObject.SetActive(true);
                }
                else {
                    gObj.CountLabel.text = remainingCounts[itemType].ToString();
                }
            }
        }

        CheckAllGoalsCompleted();
    }

    public bool CheckAllGoalsCompleted() {
        foreach (var count in remainingCounts.Values) {
            if ( count > 0)
                return false;
        }

        allGoalsCompleted = true;
        OnGoalsCompleted?.Invoke();
        return true;
    }

}
