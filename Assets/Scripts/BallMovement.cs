﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 20));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
