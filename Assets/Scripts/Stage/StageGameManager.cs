using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class StageGameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private StageUIController uiController;

    [Header("Core")]
    [SerializeField] private string stageId = "Stage1";
    [SerializeField] private GameObject player;
    [SerializeField] private StageClearCondition clearCondition;

    private IPlayerControlHandler controlHandler;
    private bool isPaused = false;
    private bool isSkillAvailable = true;
    private bool isGameOver = false;
    private bool isGameClear = false;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player"); 
        }
        controlHandler = player.GetComponent<IPlayerControlHandler>();
        if (controlHandler == null)
        {
            Debug.Log("no control handler");
        }
        else
        {
           Debug.Log("control handler"); 
        }

        uiController.ShowPauseUI(false);
        uiController.ShowGameOverUI(false);
        uiController.ShowGameClearUI(false);

        var context = GameFlowManager.Instance?.GetStageContext();
        if (context != null)
        {
            uiController.SetSkill(context.Skill);
        }
    }

    void Update()
    {
        UpdateTimerUI();
        UpdateSkillUI();
        CheckGameClear();

        // 리팩토링할 때 키보드 입력 직접 안받게 수정
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    private void UpdateTimerUI()
    {
        float remaining = clearCondition.RemainingTime;
        uiController.UpdateTimer(remaining);

        if (clearCondition.TimeOver && !isGameOver && !isGameClear)
        {
            GameOver();
        }
    }

    private void UpdateSkillUI()
    {
        uiController.SetSkillAvailability(isSkillAvailable);
    }

    private void CheckGameClear()
    {
        if (isGameClear || isGameOver) return;

        if (clearCondition.IsCleared)
        {
            isGameClear = true;
            Time.timeScale = 0f;

            uiController.ShowGameClearUI(true);
            controlHandler?.EnableInput(false);
            controlHandler?.LockCamera(true);

            Debug.Log($"클리어 등급: {clearCondition.GetClearRank()}");
            GameFlowManager.Instance.ClearStage(stageId);
        }
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Time.timeScale = 0f;

        uiController.ShowGameOverUI(true);
        controlHandler?.EnableInput(false);
        controlHandler?.LockCamera(true);
    }

    public void PauseGame()
    {
        if (isGameOver || isGameClear) return;

        uiController.ShowPauseUI(true);
        isPaused = true;
        controlHandler?.EnableInput(false);
    }

    public void ResumeGame()
    {
        uiController.ShowPauseUI(false);
        isPaused = false;
        controlHandler?.EnableInput(true);
    }

    public void RestartGame()
    {
        GameFlowManager.Instance.EnterStage(stageId);
    }

    public void QuitGame()
    {
        GameFlowManager.Instance.ContinueGame();
    }
}
