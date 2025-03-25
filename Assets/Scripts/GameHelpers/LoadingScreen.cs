using TMPro;
using UnityEngine;

// Loading screen between scenes.
public class LoadingScreen : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI loadingLabel;

    public void ShowLoading(float time) {
        if (time < 0.25f) {
            loadingLabel.text = "Loading";
        } else if (time < 0.50f) {
            loadingLabel.text = "Loading.";
        } else if (time < 0.75f) {
            loadingLabel.text = "Loading..";
        } else {
            loadingLabel.text = "Loading...";
        }
    }

}
