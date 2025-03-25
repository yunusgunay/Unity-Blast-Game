using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

// GameManager manages the game's state and scenes.
public class GameManager : Singleton<GameManager> {
    [SerializeField] private LoadingScreen loadingScene;

    protected override void Awake() { base.Awake(); DontDestroyOnLoad(gameObject); }

    public async void NextLevel() {
        int lvl = PlayerPrefs.GetInt("Level", 1);
        PlayerPrefs.SetInt("Level", lvl + 1);
        
        if (lvl >= 10) {
            await TransitionToScene("MenuScene");
        } else {
            await TransitionToScene("LevelScene");
        }
    }

    public async void LoadLevelScene() {
        await TransitionToScene("LevelScene");
    }

    public async void LoadMainMenu() {
        await TransitionToScene("MenuScene");
    }

    private async Task TransitionToScene(string scene) {
        loadingScene.gameObject.SetActive(true);
        AsyncOperation op = SceneManager.LoadSceneAsync(scene);

        while (!op.isDone) {
            loadingScene.ShowLoading(op.progress);
            await Task.Yield();
        }

        loadingScene.ShowLoading(1f);
        await Task.Delay(500);
        loadingScene.gameObject.SetActive(false);
    }

}
