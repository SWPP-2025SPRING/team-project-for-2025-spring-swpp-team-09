using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject continueUnavailablePanel;
    [SerializeField] private Button confirmButton;

    public void OnNewGameClicked()
    {
        GameFlowManager.Instance.StartNewGame();
    }

    public void OnContinueClicked()
    {
        bool continued = GameFlowManager.Instance.ContinueGame();
        if (!continued && continueUnavailablePanel != null)
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

