using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceDivision_GenerateItem : MonoBehaviour
{
    public Vector2 xSize;
    public Vector2 ySize;
    public Vector2 zSize;
    public int itemNum = 10;

    private GameObject root;

    // Start is called before the first frame update
    void Start()
    {
        root = new GameObject("Root");
        for (int i = 0; i < itemNum; i++) {
            CreateObject();
        }
    }

    void CreateObject() 
    {
        var list = Enum.GetValues(typeof(PrimitiveType));
        int length = list.Length;
        int t = UnityEngine.Random.Range(0, length);
        Transform go = GameObject.CreatePrimitive((PrimitiveType)t).transform;

        float x = UnityEngine.Random.Range(xSize.x, xSize.y);
        float y = UnityEngine.Random.Range(ySize.x, ySize.y);
        float z = UnityEngine.Random.Range(zSize.x, zSize.y);

        go.SetParent(root.transform, false);
        go.localPosition = new Vector3(x, y, z);
    }
}
