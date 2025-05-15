using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectUI : MonoBehaviour
{
    public void OnStage1Clicked()
    {
        bool isFirstPlay = !PlayerPrefs.HasKey("Stage1_Cleared");

        if (isFirstPlay)
        {
            StorySceneLoader.cutsceneId = "Stage1_Enter";
            StorySceneLoader.LoadCutscene("Stage1_Enter");
        }
        else
        {
            SceneManager.LoadScene("Stage1GameScene");
        }
    }

    public void OnStage2Clicked()
    {
        bool isFirstPlay = !PlayerPrefs.HasKey("Stage2_Cleared");

        if (isFirstPlay)
        {
            StorySceneLoader.cutsceneId = "Stage2_Enter";
            StorySceneLoader.LoadCutscene("Stage2_Enter");
        }
        else
        {
            SceneManager.LoadScene("Stage2GameScene");
        }
    }

    public void OnStage3Clicked()
    {
        bool isFirstPlay = !PlayerPrefs.HasKey("Stage3_Cleared");

        if (isFirstPlay)
        {
            StorySceneLoader.cutsceneId = "Stage3_Enter";
            StorySceneLoader.LoadCutscene("Stage3_Enter");
        }
        else
        {
            SceneManager.LoadScene("Stage3GameScene");
        }
    }
}
