using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControllerD : MonoBehaviour
{
    public float moveDistance = 0.5f;
    public float moveSpeed = 1.0f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * moveSpeed) * moveDistance;
        transform.position = new Vector3(startPos.x, startPos.y + offset, startPos.z);
    }
}