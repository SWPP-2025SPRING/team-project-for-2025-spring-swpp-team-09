using UnityEngine;
using UnityEngine.UI;

public class StageSelectUI : MonoBehaviour
{
    [SerializeField] private GameObject continueUnavailablePanel;
    [SerializeField] private Button confirmButton;

    private void Start()
    {
        if (continueUnavailablePanel != null)
            continueUnavailablePanel.SetActive(false);

        if (confirmButton != null)
            confirmButton.onClick.AddListener(OnConfirmClicked);
    }

    public void OnStage1Clicked()
    {
        TryEnterStage("Stage1");
    }

    public void OnStage2Clicked()
    {
        TryEnterStage("Stage2");
    }

    public void OnStage3Clicked()
    {
        TryEnterStage("Stage3");
    }

    private void TryEnterStage(string stageId)
    {
        bool success = GameFlowManager.Instance.EnterStage(stageId);
        if (!success && continueUnavailablePanel != null)
        {
            continueUnavailablePanel.SetActive(true);
        }
    }

    public void OnConfirmClicked()
    {
        if (continueUnavailablePanel != null)
        {
            continueUnavailablePanel.SetActive(false);
        }
    }
}
