using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitSpeed : MonoBehaviour
{
    public float maxSpeed;

    // Update is called once per frame
    void FixedUpdate()
    {
        Rigidbody rigidbody = this.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, maxSpeed);
    }
}
