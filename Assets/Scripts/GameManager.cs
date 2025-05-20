using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
using StarterAssets;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public TMP_Text timerText;
    public Image skill1Icon;
    public TMP_Text skill1Description;
    public GameObject pauseMenuUI;
    public GameObject gameOverUI;
    public GameObject gameClearUI;

    private bool isPaused = false;
    public bool isSkill1Available = true;
    private bool isGameOver = false;
    private bool isGameClear = false;

    private float timeRemaining = 120.0f;

    [SerializeField] private GameObject player;
    [SerializeField] private ThirdPersonController thirdPersonController;

    private StarterAssetsInputs playerInput;

    private void Start()
    {
        playerInput = player.GetComponent<StarterAssetsInputs>();
        gameOverUI.SetActive(false); 
        gameClearUI.SetActive(false);
    }

    void Update()
    {
        UpdateTimerUI();
        UpdateSkillUI();
        CheckGameClear();

        /* if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) {ResumeGame();}
            else {PauseGame();}
        } */

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    private void UpdateTimerUI()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = string.Format("Timer: {0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            if (!isGameOver && !isGameClear)
            {
                timerText.text = "00:00";
                GameOver();
            }
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

        if (player.transform.position.z >= 280f)
        {
            PlayerPrefs.SetInt("Stage1_Cleared", 1);
            SceneController.Instance.LoadDialogueThenScene("Stage1_Clear", "StageSelectScene");
            UpdateGameClearUI();
        }
    }

    public void UpdateGameClearUI()
    {
        isGameClear = true;
        Time.timeScale = 0f;
        gameClearUI.SetActive(true);

        if (playerInput != null)
        {
            playerInput.enabled = false;  
        }

        if (thirdPersonController != null)
        {
            thirdPersonController.LockCameraPosition = true;
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
        isPaused = true;

        if (playerInput != null)
            playerInput.enabled = false;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        isPaused = false;

        if (playerInput != null)
            playerInput.enabled = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Time.timeScale = 0f;
        gameOverUI.SetActive(true); 

        if (playerInput != null)
        {
            playerInput.enabled = false;  
        }

        if (thirdPersonController != null)
        {
            thirdPersonController.LockCameraPosition = true;
        }
    }
}
