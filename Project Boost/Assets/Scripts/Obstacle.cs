using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Obstacle : MonoBehaviour
{
    [Range(-1, 1)]
    [SerializeField]
    float moveFactor;

    [SerializeField] Vector3 moveVector = new Vector3(10f, 10f, 10f);
    Vector3 startPos;
    [SerializeField] private float period = 2f;
    
    private float cycles;   //number of cycles performed after time elapsed from start of the scene

    private float rawSineAngleValue;    //to understand what actually goes in the moveFactor.


    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //protect against NaN
        if (period <= Mathf.Epsilon) { return; }
        cycles = Time.time / period;
        const float tau = Mathf.PI * 2;
        rawSineAngleValue = Mathf.Sin(cycles * tau);    

        moveFactor = rawSineAngleValue;
        Vector3 offset = moveVector * moveFactor;
        transform.position = startPos + offset;
    }
}
