using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControllerSpin : MonoBehaviour
{
    public float SpinSpeed = 5;
    private Transform m_transform;

    void Awake()
    {
        m_transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        m_transform.Rotate(0, SpinSpeed * Time.deltaTime, 0);
    }
}
