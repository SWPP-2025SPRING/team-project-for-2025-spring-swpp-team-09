using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private Image tutorialImage;
    [SerializeField] private Button continueButton;

    private string tutorialId;
    
    void Start()
    {
        Debug.Log("[TutorialController] Start 진입");
        
        tutorialId = SceneController.Instance.GetPendingDialogueId();
        Debug.Log($"[TutorialController] Loaded DialogueId: {tutorialId}");

        if (!tutorialId.Contains("_Enter"))
        {
            Debug.LogWarning($"[TutorialController] 잘못된 진입 ID 감지: {tutorialId}, StageSelectScene으로 복귀");
            SceneController.Instance.ClearPendingSceneData();
            SceneController.Instance.LoadScene("StageSelectScene");
            return;
        }
        
        LoadTutorialImage(tutorialId);

        continueButton.onClick.AddListener(OnContinue);
    }

    private void LoadTutorialImage(string id)
    {
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
