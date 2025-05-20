using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectUI : MonoBehaviour
{
    public void OnStage1Clicked()
    {
        if (!PlayerPrefs.HasKey("Stage1_Cleared"))
            SceneController.Instance.LoadDialogueThenScene("Stage1_Enter", "Stage1GameScene");
        else
            SceneController.Instance.LoadScene("Stage1GameScene");
    }

    public void OnStage2Clicked()
    {
        if (!PlayerPrefs.HasKey("Stage2_Cleared"))
            SceneController.Instance.LoadDialogueThenScene("Stage2_Enter", "Stage2GameScene");
        else
            SceneController.Instance.LoadScene("Stage2GameScene");
    }

    public void OnStage3Clicked()
    {
        if (!PlayerPrefs.HasKey("Stage3_Cleared"))
            SceneController.Instance.LoadDialogueThenScene("Stage3_Enter", "Stage3GameScene");
        else
            SceneController.Instance.LoadScene("Stage3GameScene");
    }
}
