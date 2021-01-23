using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotation : MonoBehaviour
{
    public float speed;

    void Update()
    {
        transform.Rotate(0,Random.Range(speed/4,speed*4), 0);
    }
}
