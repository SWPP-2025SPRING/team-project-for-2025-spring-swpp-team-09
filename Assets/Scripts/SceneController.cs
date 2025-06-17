using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    private const string DialogueKey = "PendingDialogueId";
    private const string GameSceneKey = "PendingGameScene";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadDialogueThenScene(string dialogueId, string nextScene)
    {
        PlayerPrefs.SetString(DialogueKey, dialogueId);
        PlayerPrefs.SetString(GameSceneKey, nextScene);
        SceneManager.LoadScene("StoryScene");
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public string GetPendingDialogueId()
    {
        return PlayerPrefs.GetString(DialogueKey, null);
    }

    public string GetPendingGameScene()
    {
        return PlayerPrefs.GetString(GameSceneKey, null);
    }

    public void ClearPendingSceneData()
    {
        PlayerPrefs.DeleteKey(DialogueKey);
        PlayerPrefs.DeleteKey(GameSceneKey);
    }
}
