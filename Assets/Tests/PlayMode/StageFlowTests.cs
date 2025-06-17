using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class StageFlowTests
{
    private StageGameManager gameManager;
    private StageUIController uiController;
    private PlayerInputReader inputReader;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        PlayerPrefs.DeleteAll();

        // 1. GameFlowManager, SceneController 수동 생성
        var gameFlowGO = new GameObject("GameFlowManager");
        gameFlowGO.AddComponent<GameFlowManager>();

        var sceneCtrlGO = new GameObject("SceneController");
        sceneCtrlGO.AddComponent<SceneController>();

        Object.DontDestroyOnLoad(gameFlowGO);
        Object.DontDestroyOnLoad(sceneCtrlGO);

        PlayerPrefs.SetInt("Stage1_Played", 1);

        //Stage1GameScene으로 수정 필요
        SceneManager.LoadScene("temp");
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
        inputReader.testing = true;

        inputReader.PausePressed = true;
        yield return null;
        Assert.IsTrue(uiController.pauseMenuUI.activeSelf);
    }

    [UnityTest]
    public IEnumerator Resume_AllowsPlayerMovement()
    {
        inputReader.PausePressed = true;
        yield return null;
        Assert.IsTrue(uiController.pauseMenuUI.activeSelf);
        yield return new WaitForSeconds(1f);

        gameManager.ResumeGame();
        yield return new WaitForSeconds(1f);

        var player = GameObject.FindWithTag("Player");
        Vector3 initialPos = player.transform.position;

        inputReader.MoveInput = Vector2.up;
        yield return new WaitForSeconds(0.5f);

        Vector3 movedPos = player.transform.position;

        Assert.AreNotEqual(initialPos, movedPos, "Player did not move after resuming game");
    }

    /*
    [UnityTest]
    public IEnumerator Restart_ReloadsScene()
    {
        string before = SceneManager.GetActiveScene().name;
        gameManager.RestartGame();
        yield return new WaitForSeconds(1f);
        Assert.AreEqual(before, SceneManager.GetActiveScene().name);
    }
    */

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
        var condition = GameObject.FindObjectOfType<StageClearCondition>();
        var player = GameObject.FindWithTag("Player");
        var controller = player.GetComponent<CharacterController>();

        Assert.IsNotNull(condition);
        Assert.IsNotNull(player);
        Assert.IsNotNull(controller);

        controller.enabled = false;

        // 충분한 z좌표. 변경 필요
        player.transform.position = new Vector3(0f, 0f, 999f);

        typeof(StageClearCondition)
            .GetField("elapsed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(condition, 10f);

        controller.enabled = true;

        float timeout = 2f;
        float elapsed = 0f;

        while (!uiController.gameClearUI.activeSelf && elapsed < timeout)
        {
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        Time.timeScale = 1f;

        Assert.IsTrue(uiController.gameClearUI.activeSelf, "GameClear UI was not shown.");
        Assert.AreEqual("Rank: S", uiController.gameClearRank.text);
    }


    [UnityTest]
    public IEnumerator Timeout_TriggersGameOverUI()
    {
        var condition = GameObject.FindObjectOfType<StageClearCondition>();
        condition.TriggerTimeout();

        yield return new WaitForSecondsRealtime(1f);

        Assert.IsTrue(uiController.gameOverUI.activeSelf, "GameOver UI was not activated after timeout");
    }
}