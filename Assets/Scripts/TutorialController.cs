using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private Image tutorialImage;  // 조작 설명 이미지
    [SerializeField] private Button continueButton;
    [SerializeField] private SoundEventChannel soundEventChannel;

    private string tutorialId;  // 튜토리얼 ID (만약 Stage마다 다르게 관리할 경우)
    
    void Start()
    {
        tutorialId = SceneController.Instance.GetPendingDialogueId();  // (필요시 대화 ID 재활용 가능)
        LoadTutorialImage(tutorialId);
        PlayBGMForTutorial(tutorialId);

        continueButton.onClick.AddListener(OnContinue);
    }

    private void LoadTutorialImage(string id)
    {
        // 예시: Resources/TutorialImages/Stage1Tutorial.png
        string path = $"TutorialImages/{id}_Tutorial";
        Sprite image = Resources.Load<Sprite>(path);

        if (image != null)
        {
            tutorialImage.sprite = image;
        }
        else
        {
            Debug.LogWarning($"[TutorialPlayer] 튜토리얼 이미지 로드 실패: {path}");
        }
    }

    private void PlayBGMForTutorial(string id)
    {
        string bgmToPlay = id switch
        {
            "Stage1_Enter" => "tutorial_stage1",
            "Stage2_Enter" => "tutorial_stage2",
            "Stage3_Enter" => "tutorial_stage3",
            _ => null
        };

        if (!string.IsNullOrEmpty(bgmToPlay))
        {
            soundEventChannel.RaisePlayBGM(bgmToPlay);
        }
    }

    private void OnContinue()
    {
        string nextStageScene = SceneController.Instance.GetPendingStageScene();
        SceneController.Instance.ClearPendingStageScene();
        SceneController.Instance.LoadScene(nextStageScene);
    }
}
