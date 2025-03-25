using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

// UIManager manages the UI in the game.
public class UIManager : Singleton<UIManager> {
    [Header("Level Completed")]
    [SerializeField] private TextMeshProUGUI levelNumberTextCompleted;
    [SerializeField] private GameObject levelCompletedPanel;
    [SerializeField] private Button nextButton;

    [Header("Level Failed")]
    [SerializeField] private TextMeshProUGUI levelNumberTextFailed;
    [SerializeField] private GameObject levelFailedPanel;
    [SerializeField] private Button retryButton;

    [Header("Star UI")]
    [SerializeField] private Image starImage;

    [Header("Close Button")]
    [SerializeField] private Button closeButton;

    private void OnEnable() {
        MovesTracker.Instance.OnNoMovesLeft += CheckGoals;

        if (closeButton != null) {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(HandleCloseButton);
        }
    }

    private void OnDisable() {
        if (MovesTracker.Instance != null) {
            MovesTracker.Instance.OnNoMovesLeft -= CheckGoals;
        }
    }

    private void CheckGoals() {
        if (GoalManager.Instance.CheckAllGoalsCompleted()) {
            SetLevelCompletedPanel();
        } else {
            SetLevelLostPanel();
        }
    }

    public void SetLevelCompletedPanel() {
        levelNumberTextCompleted.text = "Level " + PlayerPrefs.GetInt("Level", 1);
        levelCompletedPanel.SetActive(true);
        
        if (GameManager.Instance == null) {
             throw new System.Exception("ILLEGAL STATE: Please load the game from MainScene.");
        }

        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(() => GameManager.Instance.NextLevel());
        
        starImage.transform.DOScale(1.5f, 1f);
        starImage.transform.DORotate(new Vector3(0f, 0f, 360f), 1f, RotateMode.FastBeyond360);
    }

    public void SetLevelLostPanel() {
        levelNumberTextFailed.text = "Level " + PlayerPrefs.GetInt("Level", 1);
        levelFailedPanel.SetActive(true);

        if (GameManager.Instance == null) {
            throw new System.Exception("ILLEGAL STATE: Please load the game from MainScene.");
        }

        retryButton.onClick.RemoveAllListeners();
        retryButton.onClick.AddListener(() => GameManager.Instance.LoadLevelScene());
    }

    private void HandleCloseButton() {
        if (GameManager.Instance == null) {
            throw new System.Exception("ILLEGAL STATE: Please load the game from MainScene.");
        }

        levelFailedPanel.SetActive(false);
        GameManager.Instance.LoadMainMenu();
    }

}