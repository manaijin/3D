using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class CustomMeshData : MonoBehaviour
{
    public Material mat;
    [Range(4, 30)]
    public int splitNum = 5;
    private int preNum = 5;
    [Range(5, 100)]
    public float radius = 10;
    private float preRadius = 10;

    private Mesh mesh;
    private Transform pool;
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        pool = new GameObject("Pool").transform;
        pool.localScale = Vector3.zero;
        //CreateRectangle();
        CreateSphere();
    }

    private List<int> showTrangle = new List<int>();
    private bool isAdd = true;
    void FixedUpdate()
    {
        // 重建网格
        if (splitNum != preNum || radius != preRadius)
        {
            CreateSphere();
            showTrangle.Clear();
            isAdd = true;
            preNum = splitNum;
            radius = preRadius;
        }


        // 网格渐显
        int showCount = showTrangle.Count;
        int allCount = allTrangle.Count;
        if (showCount >= allCount)
        {
            isAdd = false;
        }

        if (showCount <= 0)
        {
            isAdd = true;
        }

        try
        {
            if (isAdd)
            {
                showTrangle.Add(allTrangle[showCount]);
                showTrangle.Add(allTrangle[showCount + 1]);
                showTrangle.Add(allTrangle[showCount + 2]);
            }
            else
            {
                showTrangle.RemoveAt(showCount - 1);
                showTrangle.RemoveAt(showCount - 2);
                showTrangle.RemoveAt(showCount - 3);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }


        mesh.triangles = showTrangle.ToArray();
    }

    void CreateSphere()
    {
        CreateVertices();
        CreateTriangle();
        CreateSphereMesh();
        DrawPoint();
    }

    private List<Transform> spheres = new List<Transform>();
    void DrawPoint()
    {
        int count = spheres.Count;
        for (int i = count - 1; i >= 0; i--)
        {
            var item = spheres[i];
            spheres.Remove(item);
            RemoveToPool(item);
        }

        foreach (var pos in allPos)
        {
            Transform go = TryGetSphereFromPool();
            go.SetParent(transform);
            go.localPosition = pos;
            spheres.Add(go);
        }
    }


    private List<Vector3> allPos = new List<Vector3>();
    List<Vector3> CreateVertices()
    {
        allPos.Clear();
        float delta = Mathf.PI / splitNum;

        allPos.Add(new Vector3(0, 0, radius));
        for (int z = 1; z < splitNum; z++)
        {
            float seth = z * delta;
            for (int x = 0; x < splitNum * 2; x++)
            {
                float gama = x * delta;
                allPos.Add(new Vector3(radius * Mathf.Sin(seth) * Mathf.Cos(gama),
                            radius * Mathf.Sin(seth) * Mathf.Sin(gama),
                            radius * Mathf.Cos(seth)));
            }
        }
        allPos.Add(new Vector3(0, 0, -radius));
        return allPos;
    }

    private List<int> allTrangle = new List<int>();
    List<int> CreateTriangle()
    {
        int vertCount = allPos.Count;
        allTrangle.Clear();
        // 头
        for (int i = 1; i < splitNum * 2; i++)
        {
            allTrangle.Add(0);
            allTrangle.Add(i);
            allTrangle.Add(i + 1);
        }
        allTrangle.Add(0);
        allTrangle.Add(splitNum * 2);
        allTrangle.Add(1);

        // 中部
        for (int i = 0; i < splitNum - 2; i++)
        {
            int index1, index2, index3, index4;
            for (int j = 0; j < splitNum * 2; j++)
            {
                index1 = i * splitNum * 2 + j + 1;
                if (j != splitNum * 2 - 1)
                    index2 = index1 + 1;
                else
                    index2 = i * splitNum * 2 + 1;
                index3 = index1 + splitNum * 2;
                index4 = index2 + splitNum * 2;

                allTrangle.Add(index1);
                allTrangle.Add(index4);
                allTrangle.Add(index2);

                allTrangle.Add(index1);
                allTrangle.Add(index3);
                allTrangle.Add(index4);
            }
        }

        // 尾
        for (int i = splitNum * 2 * (splitNum - 2) + 1; i < splitNum * 2 * (splitNum - 1); i++)
        {
            allTrangle.Add(vertCount - 1);
            allTrangle.Add(i + 1);
            allTrangle.Add(i);

        }
        allTrangle.Add(vertCount - 1);
        allTrangle.Add(splitNum * 2 * (splitNum - 2) + 1);
        allTrangle.Add(vertCount - 2);

        return allTrangle;
    }

    void CreateSphereMesh()
    {
        mesh.Clear();
        mesh.vertices = allPos.ToArray();
        //mesh.triangles = allTrangle.ToArray();
        GetComponent<MeshRenderer>().material = mat;
    }

    // 创建矩形面片
    void CreateRectangle()
    {
        mesh.vertices = new Vector3[4] {
            new Vector3 (-1, -1, 0),
            new Vector3 (-1, 1, 0),
            new Vector3 (1, 1, 0),
            new Vector3 (1, -1, 0),
        };
        mesh.triangles = new int[6] {
            0, 1, 2,
            2, 3, 0
        };
        GetComponent<MeshRenderer>().material = mat;
    }

    Transform TryGetSphereFromPool()
    {
        Transform result;
        if (pool.childCount == 0)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            result = go.transform;
        }
        else
        {
            result = pool.GetChild(0);
        }
        result.SetParent(null);
        result.localScale = Vector3.one;
        return result;
    }

    void RemoveToPool(Transform t)
    {
        t.SetParent(pool, false);
    }
}
