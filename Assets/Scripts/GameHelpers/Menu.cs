using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Main Menu of the game.
public class Menu : MonoBehaviour {
    [SerializeField] private Button levelButton;
    [SerializeField] private TextMeshProUGUI levelText;
    
    private void Awake() {
        int level = PlayerPrefs.GetInt("Level", 1);
        
        if (level >= 1 && level <= 10) {
            levelText.text = "Level " + level;
            levelButton.onClick.RemoveAllListeners();
            levelButton.onClick.AddListener(() => GameManager.Instance.LoadLevelScene());
        } else {
            levelText.text = "Finished";
            levelButton.CancelInvoke();
        }
    }

}
