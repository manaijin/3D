using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetMaterialData : MonoBehaviour
{
    public Material mat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!mat) return;
        mat.SetFloat("_Stencil", 125 - transform.position.z);
    }
}
