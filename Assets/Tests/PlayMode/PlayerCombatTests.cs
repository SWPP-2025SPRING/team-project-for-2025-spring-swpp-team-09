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

    // Enemy Animation Controller 도입 이후 가능
    /*
    [UnityTest]
    public IEnumerator Enemy_Takes_Damage_Then_Dies()
    {
        // 첫 번째 공격
        int hpBefore = enemyController.CurrentHP;

        inputReader.MeleePressed = true;
        yield return null;
        inputReader.MeleePressed = false;
        yield return new WaitForSeconds(0.5f);

        int hpAfter = enemyController.CurrentHP;
        Assert.Less(hpAfter, hpBefore, "Enemy HP did not decrease after first attack.");

        // 두 번째 공격 → 사망
        inputReader.MeleePressed = true;
        yield return null;
        inputReader.MeleePressed = false;
        yield return new WaitForSeconds(0.6f);

        Assert.IsTrue(enemyController.IsDead, "Enemy not marked as dead.");
        Assert.IsFalse(enemy != null && enemy.activeInHierarchy, "Enemy GameObject still active.");
    }
    */

    [UnityTest]
    public IEnumerator Player_Speed_Halved_On_Enemy_Collision_UsingEventChannel()
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

        // 충돌이 발생할 때까지 최대 2초 대기
        float timeout = 2f;
        float elapsed = 0f;
        while (!collisionOccurred && elapsed < timeout)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        Assert.IsTrue(collisionOccurred, "Player did not collide with Enemy (OnPlayerHit not called).");

        Vector3 start = player.transform.position;
        yield return new WaitForSeconds(0.5f);
        Vector3 end = player.transform.position;

        inputReader.MoveInput = Vector2.zero;
        collisionChannel.OnPlayerHit.RemoveListener(listener); // 리스너 정리

        float distanceAfter = Vector3.Distance(start, end);
        inputReader.testing = false;

        // 플레이어 이동 속도 10f 가정
        float maxExpectedDistance = 2.5f;
        Assert.LessOrEqual(distanceAfter, maxExpectedDistance,
            $"Player did not slow down after collision. Moved {distanceAfter:F2} units.");
    }

}