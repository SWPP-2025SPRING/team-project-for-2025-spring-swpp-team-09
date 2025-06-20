using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageUIController : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text timerText;
    public Image skillIcon;
    public GameObject pauseMenuUI;
    public GameObject gameOverUI;
    public GameObject gameClearUI;
    public TMP_Text gameClearRank;

    private ISkill currentSkill;

    public void SetSkill(ISkill skill)
    {
        currentSkill = skill;
    }

    public void SetSkillAvailability(bool available)
    {
        skillIcon.color = available ? Color.white : Color.gray;
    }

    public void UpdateTimer(float seconds)
    {
        int minutes = Mathf.FloorToInt(seconds / 60);
        int secs = Mathf.FloorToInt(seconds % 60);
        timerText.text = $"Timer: {minutes:00}:{secs:00}";
    }

    public void ShowGameOverUI(bool show)
    {
        gameOverUI.SetActive(show);
    }

    public void ShowGameClearUI(bool show)
    {
        gameClearUI.SetActive(show);
    }

    public void ShowPauseUI(bool show)
    {
        pauseMenuUI.SetActive(show);
    }

    public void SetClearRank(bool show, string rank)
    {
        gameClearRank.gameObject.SetActive(show);
        if (gameClearRank != null)
            gameClearRank.text = $"Rank: {rank}";
    }
}
