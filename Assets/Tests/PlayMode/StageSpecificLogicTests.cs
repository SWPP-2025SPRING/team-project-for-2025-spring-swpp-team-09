using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class StageSpecificLogicTests
{
    private GameObject player;
    private PlayerInputReader inputReader;

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        yield return null;
    }

    [UnityTest]
    public IEnumerator Stage1_WaterContact_TriggersGameOver()
    {
        SceneManager.LoadScene("Stage1GameScene");
        yield return new WaitForSeconds(1f);

        player = GameObject.FindWithTag("Player");
        inputReader = player.GetComponent<PlayerInputReader>();

        // 강제로 물 위치로 이동
        GameObject water = GameObject.FindWithTag("Water");
        Assert.IsNotNull(water, "Water object not found.");
        player.transform.position = water.transform.position + Vector3.up * 0.5f;

        yield return new WaitForSeconds(1.0f); // 물에 닿는 처리 대기

        GameObject gameOverUI = GameObject.Find("GameOverUI");
        Assert.IsTrue(gameOverUI.activeInHierarchy, "Game over UI should be active after touching water.");
    }
    
    [UnityTest]
    public IEnumerator Stage2_WallClimbSkill_MovesPlayerUp()
    {
        SceneManager.LoadScene("Stage2GameScene");
        yield return new WaitForSeconds(1f);

        player = GameObject.FindWithTag("Player");
        inputReader = player.GetComponent<PlayerInputReader>();

        Vector3 startPos = player.transform.position;
        inputReader.testing = true;
        inputReader.SkillPressed = true;

        yield return new WaitForSeconds(1.0f); // 스킬 반응 시간

        Vector3 endPos = player.transform.position;
        Assert.Greater(endPos.y, startPos.y, "Player should have moved up the wall.");
    }

    [UnityTest]
    public IEnumerator Stage3_TimeStopSkill_FreezesOtherObjects()
    {
        SceneManager.LoadScene("Stage3GameScene");
        yield return new WaitForSeconds(1f);

        player = GameObject.FindWithTag("Player");
        inputReader = player.GetComponent<PlayerInputReader>();

        GameObject[] movingObjects = GameObject.FindGameObjectsWithTag("Movable");

        Vector3[] beforePositions = new Vector3[movingObjects.Length];
        for (int i = 0; i < movingObjects.Length; i++)
            beforePositions[i] = movingObjects[i].transform.position;

        inputReader.testing = true;
        inputReader.SkillPressed = true;

        yield return new WaitForSeconds(1.0f); // 시간 정지 효과 반영 대기

        for (int i = 0; i < movingObjects.Length; i++)
        {
            Vector3 after = movingObjects[i].transform.position;
            Assert.AreEqual(beforePositions[i], after, $"Object {i} should have stopped moving.");
        }
    }
}
