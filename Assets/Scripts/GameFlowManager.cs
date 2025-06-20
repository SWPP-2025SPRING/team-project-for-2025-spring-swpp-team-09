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
        bool anyPlayed =
            PlayerPrefs.HasKey("Stage1_Played") ||
            PlayerPrefs.HasKey("Stage2_Played") ||
            PlayerPrefs.HasKey("Stage3_Played");

        if (anyPlayed)
        {
            SceneController.Instance.LoadScene("StageSelectScene");
        }
        else
        {
            Debug.LogWarning("[GameFlowManager] 이어할 수 있는 기록이 없습니다.");
            // UIManager.Instance.ShowPopup("이어할 수 있는 저장 정보가 없습니다.");
        }
    }

    public void EnterStage(string stageId)
    {
        //if (!IsStageUnlocked(stageId))
        //{
        //    Debug.LogWarning($"[GameFlowManager] {stageId}은(는) 잠겨 있어 진입할 수 없습니다.");
        //    return;
        //}

        ISkill skill = stageId switch
        {
            "Stage2" => new WallWalkSkill(),
            "Stage3" => new TimeStopSkill(),
            _ => null
        };
        currentStageContext = new StageContext(stageId, skill);

        string playedKey = $"{stageId}_Played";
        bool alreadyPlayed = PlayerPrefs.HasKey(playedKey);

        if (alreadyPlayed)
        {
            SceneController.Instance.LoadScene($"{stageId}GameScene");
        }
        else
        {
            SceneController.Instance.LoadDialogueThenScene($"{stageId}_Enter", $"{stageId}GameScene");
        }

        PlayerPrefs.SetInt(playedKey, 1);
    }

    public void ClearStage(string stageId)
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        string key = $"{stageId}_Cleared";
        bool alreadyCleared = PlayerPrefs.HasKey(key);

        PlayerPrefs.SetInt(key, 1);

        if (alreadyCleared)
        {
            SceneController.Instance.LoadScene("StageSelectScene");
        }
        else
        {
            SceneController.Instance.LoadDialogueThenScene($"{stageId}_Clear", "StageSelectScene");
        }
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

    private bool IsStageUnlocked(string stageId)
    {
        return stageId switch
        {
            "Stage1" => true,
            "Stage2" => PlayerPrefs.HasKey("Stage1_Cleared"),
            "Stage3" => PlayerPrefs.HasKey("Stage1_Cleared") && PlayerPrefs.HasKey("Stage2_Cleared"),
            _ => false
        };
    }

    public StageContext GetStageContext()
    {
        return currentStageContext;
    }
    
#if UNITY_EDITOR
    public void EnterStageForTest(string sceneName, string stageId)
    {
        ISkill skill = stageId switch
        {
            "Stage2" => new WallWalkSkill(),
            "Stage3" => new TimeStopSkill(),
            _ => null
        };
        currentStageContext = new StageContext(stageId, skill);

        SceneManager.LoadScene(sceneName);
    }
#endif
}
