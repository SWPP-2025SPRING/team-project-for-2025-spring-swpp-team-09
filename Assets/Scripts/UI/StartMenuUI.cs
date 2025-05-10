using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuUI : MonoBehaviour
{
    public void OnNewGameClicked()
    {
        PlayerPrefs.DeleteAll();
        StorySceneLoader.LoadCutscene("Prologue");
    }

    public void OnContinueClicked()
    {
        SceneManager.LoadScene("StageSelectScene");
    }
}
