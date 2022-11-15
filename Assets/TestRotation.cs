using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotation : MonoBehaviour
{
    Transform from;
    [SerializeField] Transform to;
    void Update()
    {
        //transform.rotation *= Quaternion.AngleAxis(90  * Time.deltaTime, Vector3.right);
        Vector3 direction = to.position - transform.position;
        direction.Normalize();
        Quaternion tr = Quaternion.LookRotation(direction, Vector3.up);   
        transform.rotation = Quaternion.Slerp(transform.rotation, tr , 2f);
    }
}
