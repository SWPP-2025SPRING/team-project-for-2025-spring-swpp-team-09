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
        SaveManager.Instance.ResetAll();
        SceneController.Instance.LoadDialogueThenScene("Prologue", "StageSelectScene");
    }

    public void ContinueGame()
    {
        bool anyPlayed =
            SaveManager.Instance.IsStagePlayed("Stage1") ||
            SaveManager.Instance.IsStagePlayed("Stage2") ||
            SaveManager.Instance.IsStagePlayed("Stage3");

        if (anyPlayed)
        {
            SceneController.Instance.LoadScene("StageSelectScene");
        }
        else
        {
            Debug.LogWarning("[GameFlowManager] 이어할 수 있는 기록이 없습니다.");
        }
    }

    public void EnterStage(string stageId)
    {
        if (!IsStageUnlocked(stageId))
        {
            Debug.LogWarning($"[GameFlowManager] {stageId}은(는) 잠겨 있어 진입할 수 없습니다.");
            return;
        }

        ISkill skill = stageId switch
        {
            "Stage2" => new WallWalkSkill(),
            "Stage3" => new TimeStopSkill(),
            _ => null
        };
        Debug.Log($"[GameFlowManager] Retrieved stageId: {stageId}");
        currentStageContext = new StageContext(stageId, skill);

        bool alreadyPlayed = SaveManager.Instance.IsStagePlayed(stageId);

        if (alreadyPlayed)
        {
            SceneController.Instance.LoadScene($"{stageId}GameScene");
        }
        else
        {
            SceneController.Instance.LoadDialogueThenScene($"{stageId}_Enter", $"{stageId}GameScene");
        }

        SaveManager.Instance.SaveStagePlayed(stageId);
    }

    public void ClearStage(string stageId, float clearTime, string clearRank)
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        bool alreadyCleared = SaveManager.Instance.IsStageCleared(stageId);
        SaveManager.Instance.SaveStageClear(stageId);
        SaveManager.Instance.SaveBestTimeIfBetter(stageId, clearTime);
        SaveManager.Instance.SaveClearRank(stageId, clearRank);

        if (alreadyCleared)
        {
            SceneController.Instance.LoadScene("StageSelectScene");
        }
        else
        {
            SceneController.Instance.LoadDialogueThenScene($"{stageId}_Clear", "StageSelectScene");
        }
    }

    public void GameOver(string stageId)
    {
        SaveManager.Instance.SaveClearRank(stageId, "F");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (currentStageContext == null) return;

        string stageId = currentStageContext.StageId;
        var player = FindObjectOfType<PlayerController>();

        if (player != null && currentStageContext.Skill != null)
        {
            if (stageId == "Stage1")
            {
                return;
            }
            player.SetSkill(currentStageContext.Skill);
        }
    }

    private bool IsStageUnlocked(string stageId)
    {
        return stageId switch
        {
            "Stage1" => true,
            "Stage2" => SaveManager.Instance.IsStageCleared("Stage1"),
            "Stage3" => SaveManager.Instance.IsStageCleared("Stage1") && SaveManager.Instance.IsStageCleared("Stage2"),
            _ => false
        };
    }

    public StageContext GetStageContext()
    {
        return currentStageContext;
    }
    
    public StageRecord GetStageRecord(string stageId)
    {
        var isPlayed = SaveManager.Instance.IsStagePlayed(stageId);
        var isCleared = SaveManager.Instance.IsStageCleared(stageId);
        var rank = SaveManager.Instance.GetClearRank(stageId);
        var bestTime = SaveManager.Instance.GetBestTime(stageId);

        return new StageRecord(isPlayed, isCleared, rank, bestTime);
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

        SceneManager.sceneLoaded += InjectSkillIfAvailableForTest;
        SceneManager.LoadScene(sceneName);
    }

    private void InjectSkillIfAvailableForTest(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= InjectSkillIfAvailableForTest;

        if (currentStageContext?.Skill == null) return;

        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            var input = player.GetComponent<PlayerInputReader>();
            var move = player.GetComponent<MovementController>();
            var skillCtrl = player.GetComponent<SkillController>();

            skillCtrl.Initialize(
                currentStageContext.Skill,
                input,
                move,
                skillCtrl,
                skillCtrl.NotifySkillEnded
            );
        }
    }
#endif
}
