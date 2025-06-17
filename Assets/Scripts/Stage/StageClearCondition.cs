using UnityEngine;

public class StageClearCondition : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float requiredZ = 280f;
    [SerializeField] private float requiredY = 0f;
    [SerializeField] private bool checkY = false;

    private const float TimeLimit = 120f;
    private float elapsed = 0f;

    public bool IsCleared
    {
        get
        {
            if (elapsed > TimeLimit) return false;

            if (checkY)
            {
                return player.position.z >= requiredZ && player.position.y >= requiredY;
            }
            else
            {
                return player.position.z >= requiredZ;
            }
        }
    }

    public float RemainingTime => Mathf.Max(0f, TimeLimit - elapsed);
    public float ElapsedTime => elapsed;

    void Update()
    {
        elapsed += Time.deltaTime;
    }

    public string GetClearRank()
    {
        if (elapsed <= TimeLimit * 0.5f) return "S";
        else if (elapsed <= TimeLimit * 0.75f) return "A";
        else return "B";
    }

    public bool TimeOver => elapsed > TimeLimit;

    // 테스트용 수동 타임아웃 트리거
    public void TriggerTimeout()
    {
        elapsed = TimeLimit + 1f;
    }
}
