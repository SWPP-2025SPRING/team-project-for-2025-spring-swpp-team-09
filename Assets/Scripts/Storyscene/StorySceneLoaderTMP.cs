using UnityEngine.SceneManagement;

public static class StorySceneLoaderTMP
{
    public static string cutsceneId;

    public static void LoadCutscene(string id)
    {
        cutsceneId = id;
        SceneManager.LoadScene("StoryScene");
    }
}
