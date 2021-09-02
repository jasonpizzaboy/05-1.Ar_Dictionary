using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float rotationSpeed = 30f;
    
    void FixedUpdate()
    {
        transform.Rotate( 0f,rotationSpeed * Time.deltaTime, 0f);
        //transform.RotateAround(, Vector3.up, rotationSpeed*Time.deltaTime);
    }
}
