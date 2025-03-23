using UnityEngine;

public class LevelCompleted : MonoBehaviour
{
    [SerializeField] private GameBoard board;

    private void OnEnable()
    {
        GoalManager.Instance.OnGoalsCompleted += HandleGoalsCompleted;
    }

    private void OnDisable()
    {
        GoalManager.Instance.OnGoalsCompleted -= HandleGoalsCompleted;
    }

    private void HandleGoalsCompleted()
    {
        UIManager.Instance.SetLevelCompletedPanel();
    }
}
