using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class PlayerMovementTests
{
    private GameObject player;
    private PlayerInputReader inputReader;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        SceneManager.LoadScene("PlayerControlTestScene");
        yield return new WaitForSeconds(1f);

        player = GameObject.FindWithTag("Player");
        Assert.IsNotNull(player, "Player not found in scene.");

        inputReader = player.GetComponent<PlayerInputReader>();
        Assert.IsNotNull(inputReader, "PlayerInputReader not found.");
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        ResetInputs();
        yield return null;
    }

    [UnityTest]
    public IEnumerator MoveRight_IncreasesX()
    {
        inputReader.testing = true;
        Vector3 start = player.transform.position;

        inputReader.MoveInput = Vector2.right;
        yield return new WaitForSeconds(0.5f);
        inputReader.MoveInput = Vector2.zero;

        Vector3 end = player.transform.position;
        Assert.Greater(end.x, start.x, "Player did not move right");
    }

    [UnityTest]
    public IEnumerator MoveLeft_DecreasesX()
    {
        inputReader.testing = true;
        Vector3 start = player.transform.position;

        inputReader.MoveInput = Vector2.left;
        yield return new WaitForSeconds(0.5f);
        inputReader.MoveInput = Vector2.zero;

        Vector3 end = player.transform.position;
        Assert.Less(end.x, start.x, "Player did not move left");
    }

    [UnityTest]
    public IEnumerator MoveForward_IncreasesZ()
    {
        inputReader.testing = true;
        Vector3 start = player.transform.position;

        inputReader.MoveInput = Vector2.up;
        yield return new WaitForSeconds(0.5f);
        inputReader.MoveInput = Vector2.zero;

        Vector3 end = player.transform.position;
        Assert.Greater(end.z, start.z, "Player did not move forward");
    }

    [UnityTest]
    public IEnumerator MoveBackward_DecreasesZ()
    {
        inputReader.testing = true;
        Vector3 start = player.transform.position;

        inputReader.MoveInput = Vector2.down;
        yield return new WaitForSeconds(0.5f);
        inputReader.MoveInput = Vector2.zero;

        Vector3 end = player.transform.position;
        Assert.Less(end.z, start.z, "Player did not move backward");
    }

    [UnityTest]
    public IEnumerator Jump_Makes_Y_Increase()
    {
        float startY = player.transform.position.y;

        inputReader.JumpPressed = true;
        yield return null;
        inputReader.JumpPressed = false;

        yield return new WaitForSeconds(0.5f);

        float endY = player.transform.position.y;
        Assert.Greater(endY, startY, "Player did not jump.");
    }

    [UnityTest]
    public IEnumerator Double_Jump_Triggers_In_Air()
    {
        // First jump
        inputReader.JumpPressed = true;
        yield return null;
        inputReader.JumpPressed = false;
        yield return new WaitForSeconds(0.4f);

        float midY = player.transform.position.y;

        // Second jump
        inputReader.JumpPressed = true;
        yield return null;
        inputReader.JumpPressed = false;
        yield return new WaitForSeconds(0.4f);

        float endY = player.transform.position.y;
        Assert.Greater(endY, midY, "Double jump did not increase height.");
    }

    [UnityTest]
    public IEnumerator Dash_Makes_Short_Burst()
    {
        Vector3 start = player.transform.position;

        inputReader.DashPressed = true;
        yield return null;
        inputReader.DashPressed = false;
        yield return new WaitForSeconds(0.4f);

        Vector3 end = player.transform.position;
        float dashDist = Vector3.Distance(start, end);
        Assert.Greater(dashDist, 1f, "Dash movement too short.");
    }

    [UnityTest]
    public IEnumerator Dash_Cooldown_Prevents_Immediate_Reuse()
    {
        Vector3 start = player.transform.position;

        inputReader.DashPressed = true;
        yield return null;
        inputReader.DashPressed = false;
        yield return new WaitForSeconds(0.2f);

        Vector3 afterFirstDash = player.transform.position;

        inputReader.DashPressed = true;
        yield return null;
        inputReader.DashPressed = false;
        yield return new WaitForSeconds(0.5f);

        Vector3 afterSecondAttempt = player.transform.position;
        float secondDashDistance = Vector3.Distance(afterFirstDash, afterSecondAttempt);

        Assert.Less(secondDashDistance, 1f, "Second dash executed before cooldown ended.");
    }

    private void ResetInputs()
    {
        inputReader.MoveInput = Vector2.zero;
        inputReader.JumpPressed = false;
        inputReader.DashPressed = false;
        inputReader.testing = false;
    }
}
