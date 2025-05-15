using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControllerR : MonoBehaviour
{
    public float moveDistance = 4f;   
    public float moveSpeed = 1.5f;    

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * moveSpeed) * moveDistance;
        transform.position = new Vector3(startPos.x + offset, startPos.y, startPos.z);
    }
}
