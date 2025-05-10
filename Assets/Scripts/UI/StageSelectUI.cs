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

    // 향후 Stage2 등 추가
}
