using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuUI : MonoBehaviour
{
    public void OnNewGameClicked()
    {
        GameFlowManager.Instance.StartNewGame();
    }

    public void OnContinueClicked()
    {
        GameFlowManager.Instance.ContinueGame();
    }
}

