using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
using Cinemachine;

public class StageGameManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text timerText;
    public Image skill1Icon;
    public TMP_Text skill1Description;
    public GameObject pauseMenuUI;
    public GameObject gameOverUI;
    public GameObject gameClearUI;

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
        gameOverUI.SetActive(false);
        gameClearUI.SetActive(false);
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
        int minutes = Mathf.FloorToInt(remaining / 60);
        int seconds = Mathf.FloorToInt(remaining % 60);
        timerText.text = $"Timer: {minutes:00}:{seconds:00}";

        if (clearCondition.TimeOver && !isGameOver && !isGameClear)
        {
            GameOver();
        }
    }

    private void UpdateSkillUI()
    {
        skill1Icon.color = isSkill1Available ? Color.white : Color.gray;
        skill1Description.text = "[Skill] Press E, Allows to walk on water";
    }

    private void CheckGameClear()
    {
        if (isGameClear || isGameOver) return;

        if (clearCondition.IsCleared)
        {
            isGameClear = true;
            UpdateGameClearUI();

            GameFlowManager.Instance.ClearStage(stageId);
        }
    }

    public void UpdateGameClearUI()
    {
        Time.timeScale = 0f;
        gameClearUI.SetActive(true);

        controlHandler?.EnableInput(false);
        controlHandler?.LockCamera(true);

        Debug.Log($"클리어 등급: {clearCondition.GetClearRank()}");
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Time.timeScale = 0f;
        gameOverUI.SetActive(true);

        controlHandler?.EnableInput(false);
        controlHandler?.LockCamera(true);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
        isPaused = true;
        controlHandler?.EnableInput(false);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
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