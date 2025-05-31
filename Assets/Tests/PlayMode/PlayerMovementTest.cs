using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class PlayerMovementTests
{
    private GameObject player;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        SceneManager.LoadScene("PlayerControlTestScene");
        yield return new WaitForSeconds(1f);       
        player = GameObject.FindWithTag("Player");
        Assert.IsNotNull(player, "Player not found in scene.");
    }

    [UnityTest]
    public IEnumerator MoveRight_IncreasesX()
    {
        var start = player.transform.position;
        yield return SimulateMove(Vector3.right);
        var end = player.transform.position;
        Assert.Greater(end.x, start.x, "Player did not move right");
    }

    [UnityTest]
    public IEnumerator MoveLeft_DecreasesX()
    {
        var start = player.transform.position;
        yield return SimulateMove(Vector3.left);
        var end = player.transform.position;
        Assert.Less(end.x, start.x, "Player did not move left");
    }

    [UnityTest]
    public IEnumerator MoveForward_IncreasesZ()
    {
        var start = player.transform.position;
        yield return SimulateMove(Vector3.forward);
        var end = player.transform.position;
        Assert.Greater(end.z, start.z, "Player did not move forward");
    }

    [UnityTest]
    public IEnumerator MoveBackward_DecreasesZ()
    {
        var start = player.transform.position;
        yield return SimulateMove(Vector3.back);
        var end = player.transform.position;
        Assert.Less(end.z, start.z, "Player did not move backward");
    }

    [UnityTest]
    public IEnumerator Jump_Makes_Y_Increase()
    {
        var controller = player.GetComponent<CharacterController>();
        yield return new WaitForSeconds(0.2f);

        var startY = player.transform.position.y;

        PressJumpKey();
        yield return new WaitForSeconds(0.5f);

        var endY = player.transform.position.y;
        Assert.Greater(endY, startY, "Player did not jump.");
    }

    [UnityTest]
    public IEnumerator Double_Jump_Triggers_In_Air()
    {
        PressJumpKey(); // 1단 점프
        yield return new WaitForSeconds(0.4f);

        var midY = player.transform.position.y;
        PressJumpKey(); // 2단 점프
        yield return new WaitForSeconds(0.4f);

        var endY = player.transform.position.y;
        Assert.Greater(endY, midY, "Double jump did not increase height.");
    }

    [UnityTest]
    public IEnumerator Dash_Makes_Short_Burst()
    {
        var start = player.transform.position;
        PressDashKey();
        yield return new WaitForSeconds(0.3f);

        var end = player.transform.position;
        Assert.Greater(end.x, start.x + 1f, "Dash movement too short.");
    }

    private IEnumerator SimulateMove(Vector3 direction)
    {
        float duration = 0.5f;
        float time = 0f;

        var controller = player.GetComponent<CharacterController>();
        Assert.IsNotNull(controller, "CharacterController not found on player");

        while (time < duration)
        {
            controller.Move(direction.normalized * 5f * Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }
    }

    private void PressJumpKey()
    {
        Input.GetKeyDown(KeyCode.Space);
    }

    private void PressDashKey()
    {
        Input.GetKeyDown(KeyCode.LeftShift);
    }
}
