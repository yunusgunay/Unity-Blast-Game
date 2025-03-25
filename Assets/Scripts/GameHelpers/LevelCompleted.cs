using UnityEngine;

public class LevelCompleted : MonoBehaviour {
    [SerializeField] private GameBoard gameBoard;

    private void OnEnable() {
        GoalManager.Instance.OnGoalsCompleted += OnAllGoalsDone;
    }

    private void OnDisable() {
        if (GoalManager.Instance != null) {
            GoalManager.Instance.OnGoalsCompleted -= OnAllGoalsDone;
        }
    }

    private void OnAllGoalsDone() {
        UIManager.Instance.SetLevelCompletedPanel();
    }

}
