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
        float waitTime = 0.5f;
        float elapsed = 0f;

        while (elapsed < waitTime)
        {
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
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
    public IEnumerator Dash_Cooldown_Is_Exactly_5_Seconds()
    {
        Vector3 start = player.transform.position;

        inputReader.DashPressed = true;
        yield return null;
        inputReader.DashPressed = false;
        yield return new WaitForSeconds(0.1f);

        Vector3 afterFirstDash = player.transform.position;

        inputReader.DashPressed = true;
        yield return null;
        inputReader.DashPressed = false;
        yield return new WaitForSeconds(0.5f);

        Vector3 afterSecondAttempt = player.transform.position;
        float earlyDashDistance = Vector3.Distance(afterFirstDash, afterSecondAttempt);
        Assert.Less(earlyDashDistance, 1f, "Dash should not be available before 5 seconds.");

        yield return new WaitForSeconds(4.5f);

        inputReader.DashPressed = true;
        yield return null;
        inputReader.DashPressed = false;
        yield return new WaitForSeconds(0.5f);

        Vector3 afterThirdAttempt = player.transform.position;
        float lateDashDistance = Vector3.Distance(afterSecondAttempt, afterThirdAttempt);
        Assert.Greater(lateDashDistance, 1f, "Dash should be available after 5 seconds.");
    }


    [UnityTest]
    public IEnumerator MoveInput_Triggers_IdleRun_Animation()
    {
        var animator = player.GetComponentInChildren<Animator>();
        Assert.IsNotNull(animator, "Animator not found.");

        inputReader.testing = true;
        inputReader.MoveInput = Vector2.right;

        yield return new WaitForSeconds(0.4f);

        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        Assert.IsTrue(stateInfo.IsName("Idle Run"),
            $"Expected animation state 'Idle Run', but was '{stateInfo.fullPathHash}'");

        inputReader.MoveInput = Vector2.zero;
    }

    [UnityTest]
    public IEnumerator Jump_Triggers_JumpStart_And_InAir_Animation()
    {
        var animator = player.GetComponentInChildren<Animator>();
        Assert.IsNotNull(animator, "Animator not found.");

        inputReader.testing = true;
        inputReader.JumpPressed = true;
        yield return null;
        inputReader.JumpPressed = false;

        float timeout = 0.5f;
        float elapsed = 0f;
        bool enteredJumpStart = false;

        while (elapsed < timeout)
        {
            var state = animator.GetCurrentAnimatorStateInfo(0);
            if (state.IsName("JumpStart"))
            {
                enteredJumpStart = true;
                break;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        Assert.IsTrue(enteredJumpStart, "Did not enter 'JumpStart' animation state.");

        timeout = 1f;
        elapsed = 0f;

        while (elapsed < timeout)
        {
            var state = animator.GetCurrentAnimatorStateInfo(0);
            if (state.IsName("InAir"))
            {
                Assert.Pass("Entered 'InAir' animation state after 'Jump Start'.");
                yield break;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        Assert.Fail("Did not enter 'InAir' animation state after 'Jump Start'.");
    }

    [UnityTest]
    public IEnumerator Landing_Triggers_JumpLand_Animation()
    {
        var animator = player.GetComponentInChildren<Animator>();
        Assert.IsNotNull(animator, "Animator not found.");

        inputReader.testing = true;

        inputReader.JumpPressed = true;
        yield return null;
        inputReader.JumpPressed = false;

        yield return new WaitForSeconds(0.4f);
        yield return new WaitUntil(() => player.GetComponent<CharacterController>().isGrounded); // 착지할 때까지 대기

        float timeout = 0.5f;
        float elapsed = 0f;
        while (elapsed < timeout)
        {
            var stateLand = animator.GetCurrentAnimatorStateInfo(0);
            if (stateLand.IsName("JumpLand"))
            {
                Assert.Pass("Entered JumpLand animation state after landing.");
                yield break;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        Assert.Fail("JumpLand animation did not play after landing.");
    }

    private void ResetInputs()
    {
        inputReader.MoveInput = Vector2.zero;
        inputReader.JumpPressed = false;
        inputReader.DashPressed = false;
        inputReader.testing = false;
    }
}
