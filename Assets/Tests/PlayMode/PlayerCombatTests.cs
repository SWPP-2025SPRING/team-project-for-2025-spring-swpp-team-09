using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class PlayerCombatTests
{
    private GameObject player;
    private PlayerInputReader inputReader;
    private GameObject enemy;
    private EnemyController enemyController;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        SceneManager.LoadScene("PlayerCombatTestScene");
        yield return new WaitForSeconds(1f);

        player = GameObject.FindWithTag("Player");
        Assert.IsNotNull(player, "Player not found.");

        inputReader = player.GetComponent<PlayerInputReader>();
        Assert.IsNotNull(inputReader, "PlayerInputReader not found.");

        enemy = GameObject.FindWithTag("Enemy");
        Assert.IsNotNull(enemy, "Enemy not found.");

        enemyController = enemy.GetComponent<EnemyController>();
        Assert.IsNotNull(enemyController, "EnemyController not found.");
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        inputReader.MeleePressed = false;
        yield return null;
    }

    [UnityTest]
    public IEnumerator Enemy_Takes_Damage_On_Attack()
    {
        int hpBefore = enemyController.CurrentHP;

        inputReader.MeleePressed = true;
        yield return null;
        inputReader.MeleePressed = false;
        yield return new WaitForSeconds(0.5f);

        int hpAfter = enemyController.CurrentHP;
        Assert.Less(hpAfter, hpBefore, "Enemy HP did not decrease after attack.");
    }

    [UnityTest]
    public IEnumerator Enemy_Dies_After_Second_Attack()
    {
        inputReader.MeleePressed = true;
        yield return null;
        inputReader.MeleePressed = false;
        yield return new WaitForSeconds(0.5f);

        inputReader.MeleePressed = true;
        yield return null;
        inputReader.MeleePressed = false;
        yield return new WaitForSeconds(2f);

        Assert.IsTrue(enemyController.IsDead, "Enemy not marked as dead.");
        Assert.IsFalse(enemy != null && enemy.activeInHierarchy, "Enemy GameObject still active.");
    }

    [UnityTest]
    public IEnumerator Player_Speed_Halved_Then_Restores_After_Enemy_Collision()
    {
        inputReader.testing = true;
        bool collisionOccurred = false;

        var handler = Object.FindObjectOfType<CollisionEventHandler>();
        var collisionChannel = handler?.GetCollisionChannel();

        Assert.IsNotNull(collisionChannel, "CollisionEventChannel not found.");

        // 충돌 이벤트 리스너 등록
        UnityEngine.Events.UnityAction<Collider, float, float> listener = (col, ratio, duration) =>
        {
            collisionOccurred = true;
        };
        collisionChannel.OnPlayerHit.AddListener(listener);

        inputReader.MoveInput = Vector2.up;

        // 충돌 발생까지 대기 (최대 2초)
        float timeout = 2f;
        float elapsed = 0f;
        while (!collisionOccurred && elapsed < timeout)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        Assert.IsTrue(collisionOccurred, "Player did not collide with Enemy (OnPlayerHit not called).");

        // 충돌 직후 이동 거리 측정
        Vector3 slowedStart = player.transform.position;
        yield return new WaitForSeconds(0.5f);
        Vector3 slowedEnd = player.transform.position;

        float slowedDistance = Vector3.Distance(slowedStart, slowedEnd);

        // 회복 시간(1.5초) 경과 후, 다시 같은 방향으로 이동
        yield return new WaitForSeconds(1.6f);
        Vector3 restoredStart = player.transform.position;
        yield return new WaitForSeconds(0.5f);
        Vector3 restoredEnd = player.transform.position;

        float restoredDistance = Vector3.Distance(restoredStart, restoredEnd);

        inputReader.MoveInput = Vector2.zero;
        collisionChannel.OnPlayerHit.RemoveListener(listener);
        inputReader.testing = false;

        // 판단 기준
        Assert.Less(slowedDistance, restoredDistance * 0.7f,
            $"Slowed movement was not significantly less. Slowed: {slowedDistance:F2}, Restored: {restoredDistance:F2}");

        Assert.Greater(restoredDistance, slowedDistance,
            $"Player speed did not recover after slow duration. Slowed: {slowedDistance:F2}, Restored: {restoredDistance:F2}");
    }

    [UnityTest]
    public IEnumerator Enemy_Moves_Towards_Player_With_Walk_Animation()
    {
        Vector3 start = enemy.transform.position;
        Vector3 target = player.transform.position;

        // 일정 시간 동안 기다리며 이동 여부 확인
        yield return new WaitForSeconds(1f);

        Vector3 end = enemy.transform.position;
        float movedDistance = Vector3.Distance(start, end);

        Assert.Greater(movedDistance, 0.1f, "Enemy did not move toward the player.");

        Animator animator = enemy.GetComponent<Animator>();
        Assert.IsNotNull(animator, "Enemy Animator not found.");

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        bool isWalking = state.IsName("Walk") || state.IsTag("Walk");

        Assert.IsTrue(isWalking, $"Enemy is not playing walk animation (current state: {state.fullPathHash}).");
    }
}