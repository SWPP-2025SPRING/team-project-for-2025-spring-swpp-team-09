using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private Image tutorialImage;  // 조작 설명 이미지
    [SerializeField] private Button continueButton;

    private string tutorialId;  // 튜토리얼 ID (만약 Stage마다 다르게 관리할 경우)
    
    void Start()
    {
        Debug.Log("[TutorialController] Start 진입");
        
        tutorialId = SceneController.Instance.GetPendingDialogueId();  // (필요시 대화 ID 재활용 가능)
        Debug.Log($"[TutorialController] Loaded DialogueId: {tutorialId}");
        LoadTutorialImage(tutorialId);

        continueButton.onClick.AddListener(OnContinue);
    }

    private void LoadTutorialImage(string id)
    {
        // 예시: Resources/TutorialImages/Stage1Tutorial.png
        string path = $"backgrounds/{id}_tutorial";
        Sprite image = Resources.Load<Sprite>(path);

        if (image != null)
        {
            tutorialImage.sprite = image;
            Debug.Log("[TutorialController] 튜토리얼 이미지 성공적으로 로드됨");
        }
        else
        {
            Debug.LogWarning($"[TutorialPlayer] 튜토리얼 이미지 로드 실패: {path}");
        }
    }

    private void OnContinue()
    {
        string nextStageScene = SceneController.Instance.GetPendingStageScene();
        SceneController.Instance.ClearPendingStageScene();
        SceneController.Instance.LoadScene(nextStageScene);
    }
}
