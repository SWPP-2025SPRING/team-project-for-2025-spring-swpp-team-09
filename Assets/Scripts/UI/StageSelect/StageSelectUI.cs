using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectUI : MonoBehaviour
{
    public void OnStage1Clicked()
    {
        GameFlowManager.Instance.EnterStage("Stage1");
    }

    public void OnStage2Clicked()
    {
        GameFlowManager.Instance.EnterStage("Stage2");
    }

    public void OnStage3Clicked()
    {
        GameFlowManager.Instance.EnterStage("Stage3");
    }
}
