using UnityEngine.SceneManagement;

public static class StorySceneLoader
{
    public static string cutsceneId;

    public static void LoadCutscene(string id)
    {
        cutsceneId = id;
        SceneManager.LoadScene("StoryScene");
    }
}
