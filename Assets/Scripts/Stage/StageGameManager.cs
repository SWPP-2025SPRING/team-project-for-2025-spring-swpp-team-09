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
    private bool isSkill1Available = true;
    private bool isGameOver = false;
    private bool isGameClear = false;

    void Start()
    {
        controlHandler = player.GetComponent<IPlayerControlHandler>();

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
        uiController.SetSkillAvailability(isSkill1Available);
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
        Time.timeScale = 0f;
        uiController.ShowPauseUI(true);
        isPaused = true;
        controlHandler?.EnableInput(false);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        uiController.ShowPauseUI(false);
        isPaused = false;
        controlHandler?.EnableInput(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
