using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class StageSpecificLogicTests
{
    private GameObject player;
    private PlayerInputReader inputReader;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        if (GameFlowManager.Instance == null)
        {
            var gameFlowGO = new GameObject("GameFlowManager");
            gameFlowGO.AddComponent<GameFlowManager>();
            Object.DontDestroyOnLoad(gameFlowGO);
        }

        if (SaveManager.Instance == null)
        {
            var saveGO = new GameObject("SaveManager");
            saveGO.AddComponent<SaveManager>();
            Object.DontDestroyOnLoad(saveGO);
        }

        if (SceneController.Instance == null)
        {
            var sceneCtrlGO = new GameObject("SceneController");
            sceneCtrlGO.AddComponent<SceneController>();
            Object.DontDestroyOnLoad(sceneCtrlGO);
        }

        yield return null;
    }


    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Time.timeScale = 1f;

        var gameFlowGO = GameObject.Find("GameFlowManager");
        if (gameFlowGO != null) Object.Destroy(gameFlowGO);

        yield return null;
    }

    [UnityTest]
    public IEnumerator Stage1_WaterContact_TriggersGameOver()
    {
        GameFlowManager.Instance.EnterStageForTest("GameTestScene", "Stage1");
        yield return new WaitForSeconds(1f);

        player = GameObject.FindWithTag("Player");
        inputReader = player.GetComponent<PlayerInputReader>();
        inputReader.testing = true;

        inputReader.MoveInput = Vector2.right;
        yield return new WaitForSeconds(0.1f);
        inputReader.MoveInput = Vector2.up;
        yield return new WaitForSecondsRealtime(1f);

        GameObject gameOverUI = GameObject.Find("GameOverText");
        Assert.IsNotNull(gameOverUI, "GameOverUI not found.");
        Assert.IsTrue(gameOverUI.activeInHierarchy, "Game over UI should be active after water contact.");
    }

    [UnityTest]
    public IEnumerator Stage2_WallClimbSkill_MovesPlayerUp()
    {
        GameFlowManager.Instance.EnterStageForTest("GameTestScene", "Stage2");
        yield return new WaitForSeconds(1f);

        player = GameObject.FindWithTag("Player");
        inputReader = player.GetComponent<PlayerInputReader>();
        inputReader.testing = true;

        Vector3 startPos = player.transform.position;

        inputReader.MoveInput = Vector2.up;
        yield return new WaitForSeconds(0.5f);
        inputReader.JumpPressed = true;
        inputReader.SkillPressed = true;
        yield return new WaitForSeconds(0.1f);

        float climbTime = 1f;
        float elapsed = 0f;

        while (elapsed < climbTime)
        {
            inputReader.SkillPressed = true;
            inputReader.MoveInput = Vector2.up;
            elapsed += Time.deltaTime;
            yield return null;
        }

        inputReader.SkillPressed = false;
        inputReader.JumpPressed = false;

        Vector3 endPos = player.transform.position;
        Assert.Greater(endPos.y, startPos.y + 0.5f, "Player should have moved upward using wall climb skill.");
    }

    [UnityTest]
    public IEnumerator Stage3_TimeStopSkill_FreezesGameTime()
    {
        GameFlowManager.Instance.EnterStageForTest("GameTestScene", "Stage3");
        yield return new WaitForSeconds(1f);

        player = GameObject.FindWithTag("Player");
        inputReader = player.GetComponent<PlayerInputReader>();
        inputReader.testing = true;

        float timeBefore = Time.time;

        inputReader.SkillPressed = true;

        yield return new WaitForSecondsRealtime(1f);

        float timeAfter = Time.time;

        Assert.AreEqual(timeBefore, timeAfter, 0.01f, "Time.time should not have progressed if time is stopped.");
    }

    [UnityTest]
    public IEnumerator Stage3_TimeStopSkill_FreezesGameTime_And_ResetsAfterCooldown()
    {
        GameFlowManager.Instance.EnterStageForTest("GameTestScene", "Stage3");
        yield return new WaitForSeconds(1f);

        player = GameObject.FindWithTag("Player");
        inputReader = player.GetComponent<PlayerInputReader>();
        inputReader.testing = true;

        inputReader.SkillPressed = true;
        yield return new WaitForSecondsRealtime(0.2f);
        Assert.AreEqual(0f, Time.timeScale, "Time should be frozen after first skill use.");

        yield return new WaitUntil(() => Time.timeScale == 1f);

        yield return new WaitForSecondsRealtime(4f);

        float timeBefore = Time.time;
        inputReader.SkillPressed = true;
        yield return new WaitForSecondsRealtime(0.2f);

        float timeAfter = Time.time;
        Assert.AreEqual(timeBefore, timeAfter, 0.01f, "Time should be frozen again after cooldown.");
    }
}
