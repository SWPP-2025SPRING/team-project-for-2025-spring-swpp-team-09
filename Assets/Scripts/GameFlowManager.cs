using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance { get; private set; }
    private StageContext currentStageContext;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
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
        ISkill skill = stageId switch
        {
            "Stage2" => new GlideSkill(),
            "Stage1" => new TimeStopSkill(),
            _ => null
        };
        currentStageContext = new StageContext(stageId, skill);

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

    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (currentStageContext == null) return;

        Debug.Log($"[GameFlowManager] Scene loaded: {scene.name}");

        var player = FindObjectOfType<PlayerController>();
        if (player != null && currentStageContext.Skill != null)
        {
            Debug.Log("[GameFlowManager] Injecting skill into PlayerController");
            player.SetSkill(currentStageContext.Skill);
        }
    }
}
