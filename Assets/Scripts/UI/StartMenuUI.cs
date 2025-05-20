using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuUI : MonoBehaviour
{
    public void OnNewGameClicked()
    {
        PlayerPrefs.DeleteAll();
        SceneController.Instance.LoadDialogueThenScene("Prologue", "StageSelectScene");
    }

    public void OnContinueClicked()
    {
        SceneController.Instance.LoadScene("StageSelectScene");
    }
}

