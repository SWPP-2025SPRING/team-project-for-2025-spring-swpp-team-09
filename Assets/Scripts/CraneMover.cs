using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneMover : MonoBehaviour
{
    [SerializeField] private float moveDistance = 5f;  // 이동 거리
    [SerializeField] private float moveSpeed = 2f;     // 이동 속도
    [SerializeField] private Vector3 moveDirection = Vector3.forward; // 앞뒤: z방향, 좌우: x방향

    private Vector3 startPos;
    private bool movingForward = true;
    
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float step = moveSpeed * Time.deltaTime;
        Vector3 targetPos = startPos + (movingForward ? moveDirection : -moveDirection) * moveDistance;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

        // 방향 전환
        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            movingForward = !movingForward;
        }
    }
}
