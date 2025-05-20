using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartNewGame()
    {
        PlayerPrefs.DeleteAll();
        SceneController.Instance.LoadDialogueThenScene("Prologue", "StageSelectScene");
    }

    public void ContinueGame()
    {
        SceneController.Instance.LoadScene("StageSelectScene");
    }

    public void EnterStage(string stageId)
    {
        string key = $"{stageId}_Cleared";
        if (PlayerPrefs.HasKey(key))
        {
            SceneController.Instance.LoadScene($"{stageId}GameScene");
        }
        else
        {
            SceneController.Instance.LoadDialogueThenScene($"{stageId}_Enter", $"{stageId}GameScene");
        }
    }

    public void ClearStage(string stageId)
    {
        PlayerPrefs.SetInt($"{stageId}_Cleared", 1);
        SceneController.Instance.LoadDialogueThenScene($"{stageId}_Clear", "StageSelectScene");
    }

    public void GameOver()
    {
        SceneController.Instance.LoadScene("GameOverScene");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
