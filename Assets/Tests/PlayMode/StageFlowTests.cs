using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
/*
public class StageFlowTests
{
    private StageGameManager gameManager;
    private StageUIController uiController;
    private PlayerInputReader inputReader;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        PlayerPrefs.DeleteAll();

        SceneManager.LoadScene("StartMenuScene");
        yield return new WaitForSeconds(0.5f);

        var startMenu = GameObject.FindObjectOfType<StartMenuUI>();
        Assert.IsNotNull(startMenu);
        startMenu.OnNewGameClicked();

        yield return new WaitForSeconds(1f);
        var dialoguePlayer = GameObject.FindObjectOfType<DialoguePlayer>();
        if (dialoguePlayer != null)
        {
            dialoguePlayer.SkipDialogue();
            yield return new WaitForSeconds(1f);
        }

        var stageUI = GameObject.FindObjectOfType<StageSelectUI>();
        Assert.IsNotNull(stageUI);
        stageUI.OnStage1Clicked();
        yield return new WaitForSeconds(1f);

        gameManager = GameObject.FindObjectOfType<StageGameManager>();
        uiController = GameObject.FindObjectOfType<StageUIController>();
        inputReader = GameObject.FindWithTag("Player").GetComponent<PlayerController>().inputReader;

        Assert.NotNull(gameManager);
        Assert.NotNull(uiController);
        Assert.NotNull(inputReader);

        inputReader.testing = true;
    }

    [UnityTest]
    public IEnumerator Pause_ShowsPauseUI()
    {
        inputReader.PausePressed = true;
        yield return null;
        Assert.IsTrue(uiController.pauseMenuUI.activeSelf);
    }

    [UnityTest]
    public IEnumerator Restart_ReloadsScene()
    {
        string before = SceneManager.GetActiveScene().name;
        gameManager.RestartGame();
        yield return new WaitForSeconds(1f);
        Assert.AreEqual(before, SceneManager.GetActiveScene().name);
    }

    [UnityTest]
    public IEnumerator Quit_LeadsToStageSelect()
    {
        gameManager.QuitGame();
        yield return new WaitForSeconds(1f);
        Assert.AreEqual("StageSelectScene", SceneManager.GetActiveScene().name);
    }

    [UnityTest]
    public IEnumerator GameClear_ShowsRank()
    {
        var condition = GameObject.FindObjectOfType<StageClearConditionFake>();
        condition.TriggerClear("S");
        yield return new WaitForSeconds(0.1f);

        Assert.IsTrue(uiController.gameClearUI.activeSelf);
        Assert.AreEqual("Rank: S", uiController.gameClearRank.text);
    }

    [UnityTest]
    public IEnumerator Timeout_TriggersGameOverUI()
    {
        var condition = GameObject.FindObjectOfType<StageClearConditionFake>();
        condition.TriggerTimeout();
        yield return new WaitForSeconds(0.1f);

        Assert.IsTrue(uiController.gameOverUI.activeSelf);
    }
}
*/