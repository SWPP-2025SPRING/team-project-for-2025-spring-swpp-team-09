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
        // 예: 적어도 하나의 스테이지라도 클리어된 기록이 있어야 이어하기 허용
        bool anyCleared = PlayerPrefs.HasKey("Stage1_Cleared") || PlayerPrefs.HasKey("Stage2_Cleared");

        if (anyCleared)
        {
            SceneController.Instance.LoadScene("StageSelectScene");
        }
        else
        {
            Debug.LogWarning("[GameFlowManager] 이어할 수 있는 클리어 기록이 없습니다.");
            // UIManager.Instance.ShowPopup("이어할 수 있는 저장 정보가 없습니다.");
        }
    }

    public void EnterStage(string stageId)
    {
        ISkill skill = stageId switch
        {
            // "Stage2" => new GlideSkill(),
            "Stage2" => new WallWalkSkill(),
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
