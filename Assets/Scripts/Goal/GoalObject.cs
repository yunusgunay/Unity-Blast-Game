using TMPro;
using UnityEngine;
using UnityEngine.UI;

// This class represent goal objects in a level.
public class GoalObject : MonoBehaviour {
    [SerializeField] private Image goalImage;
    [SerializeField] private Image completionMark;
    [SerializeField] private TextMeshProUGUI countLabel;

    private int remainingAmount;
    private GoalData goalData;

    public void PrepareGoalObj(GoalData data) {
        goalData = data;
        
        goalImage.sprite = ImageConverter.Instance.getImage(data.GoalObstacle);

        remainingAmount = data.RequiredCount;
        completionMark.gameObject.SetActive(false);
        
        countLabel.text = remainingAmount.ToString();
    }

    public GoalData GetData() {
        return goalData;
    }

    public TextMeshProUGUI CountLabel {
        get { return countLabel; }
    }

    public Image CompletionMark {
        get { return completionMark; }
    }

}
