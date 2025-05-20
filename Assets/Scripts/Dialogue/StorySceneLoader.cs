using UnityEngine;
using UnityEngine.SceneManagement;

public static class StorySceneLoader
{
    private const string Key = "DialogueID";

    public static void LoadDialogue(string id)
    {
        PlayerPrefs.SetString(Key, id);
        SceneManager.LoadScene("StoryScene");
    }

    public static string GetDialogueId()
    {
        return PlayerPrefs.GetString(Key, "");
    }
}
