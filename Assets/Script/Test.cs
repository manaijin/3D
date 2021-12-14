using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform t1;
    public Transform t2;
    // Start is called before the first frame update
    void Start()
    {
        t1.position = new Vector3(10, 0, 0);
        t2.position = new Vector3(0, 0, 0);
        t1.RotateAround(t2.position, Vector3.up, 90);
        Debug.Log(t1.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
