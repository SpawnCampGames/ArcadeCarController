using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection2 : MonoBehaviour
{
    public LayerMask colLayerMask;

    public static bool frontSensor;
    public static bool rearSensor;

    public float sphereCastLength;

    void Update()
    {
        RaycastHit frontHit;
        RaycastHit rearHit;
        frontSensor = Physics.SphereCast(transform.position, 1f, transform.forward, out frontHit, sphereCastLength, colLayerMask);
        rearSensor = Physics.SphereCast(transform.position, 1f, -transform.forward, out rearHit, sphereCastLength, colLayerMask);
    }
}
