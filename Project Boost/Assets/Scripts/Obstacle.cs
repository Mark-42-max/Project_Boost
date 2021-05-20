using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Obstacle : MonoBehaviour
{
    [Range(-1, 1)]
    [SerializeField]
    float moveFactor;

    [SerializeField] Vector3 moveDistance;
    Vector3 startPos;
    

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = moveDistance * moveFactor;
        transform.position = startPos + offset;
    }
}
