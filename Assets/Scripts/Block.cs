using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private Vector3 direction = Vector3.right;
    public float speed = 5;
    public float maxX; // x value that the block is to be destroyed at
    
    void Update()
    {
        transform.Translate(Time.deltaTime * direction);
        if (transform.position.x > maxX)
        {
            Destroy(gameObject);
        }
    }
}
