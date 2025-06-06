using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageUIController : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text timerText;
    public Image skillIcon;
    public TMP_Text skillDescription;
    public GameObject pauseMenuUI;
    public GameObject gameOverUI;
    public GameObject gameClearUI;

    private ISkill currentSkill;

    // 리팩토링할 때 설명도 주입 받게 수정하기
    public void SetSkill(ISkill skill)
    {
        currentSkill = skill;

        if (skill is WallWalkSkill)
        {
            skillDescription.text = "[Skill] Press E, Walk on walls";
        }
        else if (skill is TimeStopSkill)
        {
            skillDescription.text = "[Skill] Press E, Stop time";
        }
        else
        {
            skillDescription.text = "[Skill] No skill assigned";
        }
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
}
