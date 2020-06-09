using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOffBound : MonoBehaviour
{
    public float boundeSize = 1000;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // destroy every object out of the bounderies 
        Vector3 objectposition = transform.position;
        if (objectposition.z > boundeSize || objectposition.z < -boundeSize || objectposition.x > boundeSize || objectposition.x < -boundeSize)
        {
            Destroy(gameObject);
        }
    }
}
