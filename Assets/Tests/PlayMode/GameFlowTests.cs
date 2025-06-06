using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.TestTools;

public class GameFlowTests
{
    private GameObject startMenuUIObject;
    private GameObject stageSelectUIObject;
    private StartMenuUI startMenuUI;
    private StageSelectUI stageSelectUI;
    private string loadedSceneName;

    [UnitySetUp]
    public IEnumerator UnitySetup()
    {
        PlayerPrefs.DeleteAll();
        loadedSceneName = "";

        SceneManager.sceneLoaded += OnSceneLoaded;

        startMenuUIObject = new GameObject("StartMenuUI");
        startMenuUI = startMenuUIObject.AddComponent<StartMenuUI>();

        stageSelectUIObject = new GameObject("StageSelectUI");
        stageSelectUI = stageSelectUIObject.AddComponent<StageSelectUI>();

        new GameObject("GameFlowManager").AddComponent<GameFlowManager>();
        new GameObject("SceneController").AddComponent<SceneController>();

        yield return null;
    }

    [TearDown]
    public void TearDown()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (GameFlowManager.Instance != null)
            Object.Destroy(GameFlowManager.Instance.gameObject);
        if (SceneController.Instance != null)
            Object.Destroy(SceneController.Instance.gameObject);
        if (startMenuUIObject != null)
            Object.Destroy(startMenuUIObject);
        if (stageSelectUIObject != null)
            Object.Destroy(stageSelectUIObject);

        PlayerPrefs.DeleteAll();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        loadedSceneName = scene.name;
    }

    [UnityTest]
    public IEnumerator NewGame_ProceedsToStageSelectScene_AfterSkip()
    {
        startMenuUI.OnNewGameClicked();
        yield return new WaitForSeconds(0.2f);
        Assert.AreEqual("StoryScene", loadedSceneName);

        var dialoguePlayer = Object.FindObjectOfType<DialoguePlayer>();
        Assert.IsNotNull(dialoguePlayer, "DialoguePlayer not found in StoryScene");

        dialoguePlayer.SkipDialogue();
        yield return new WaitForSeconds(0.3f);

        Assert.AreEqual("StageSelectScene", loadedSceneName);
    }

    [UnityTest]
    public IEnumerator ContinueGame_WithSaveData()
    {
        PlayerPrefs.SetInt("Stage1_Cleared", 1);
        startMenuUI.OnContinueClicked();
        yield return new WaitForSeconds(0.1f);

        Assert.AreEqual("StageSelectScene", loadedSceneName);
    }

    [UnityTest]
    public IEnumerator ContinueGame_WithoutSaveData()
    {
        startMenuUI.OnContinueClicked();
        yield return new WaitForSeconds(0.1f);

        Assert.AreEqual("", loadedSceneName);
    }

    [UnityTest]
    public IEnumerator Stage2Blocked_WhenStage1NotCleared()
    {
        PlayerPrefs.DeleteKey("Stage1_Cleared");

        stageSelectUI.OnStage2Clicked();
        yield return new WaitForSeconds(0.1f);

        Assert.AreEqual("", loadedSceneName);
    }

    [UnityTest]
    public IEnumerator Stage2Unlocked_WhenStage1Cleared()
    {
        PlayerPrefs.SetInt("Stage1_Cleared", 1);

        stageSelectUI.OnStage2Clicked();
        yield return new WaitForSeconds(0.1f);

        Assert.AreEqual("StoryScene", loadedSceneName);
    }

    [UnityTest]
    public IEnumerator FirstClear_ShowsCutscene()
    {
        PlayerPrefs.DeleteKey("Stage1_Cleared");
        GameFlowManager.Instance.ClearStage("Stage1");
        yield return new WaitForSeconds(0.1f);

        Assert.AreEqual("StoryScene", loadedSceneName);
        Assert.AreEqual(1, PlayerPrefs.GetInt("Stage1_Cleared"));
    }

    [UnityTest]
    public IEnumerator ReClear_SkipsCutscene()
    {
        PlayerPrefs.SetInt("Stage1_Cleared", 1);
        GameFlowManager.Instance.ClearStage("Stage1");
        yield return new WaitForSeconds(0.1f);

        Assert.AreEqual("StageSelectScene", loadedSceneName);
    }
}
