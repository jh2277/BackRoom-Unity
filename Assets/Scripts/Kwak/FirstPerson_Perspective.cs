using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPerson_Perspective : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform playerTransform;
    public Vector3 offset; //플레이어와 카메라 사이의 거리
    void Awake()
    {
        offset = transform.position - playerTransform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = playerTransform.position + offset;
    }
}
