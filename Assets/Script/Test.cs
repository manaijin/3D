using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public SkinnedMeshRenderer sk;
    // Start is called before the first frame update
    void Start()
    {
        if (!sk) return;
        GameObject go = new GameObject("Bake");
        var f = go.AddComponent<MeshFilter>();
        var r = go.AddComponent<MeshRenderer>();
        go.transform.position = transform.position;
        Mesh m = new Mesh();
        sk.BakeMesh(m);
        f.mesh = m;
        r.materials = sk.materials;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
