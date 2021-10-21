using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 随机生成一定数量、类型对象，测试场景管理算法
/// </summary>
public class SpaceDivision_GenerateItem : MonoBehaviour
{
    public Vector2 xRange = new Vector2(-10, 10);
    public Vector2 yRange = new Vector2(-10, 10);
    public Vector2 zRange = new Vector2(-10, 10);
    public int itemNum = 10;

    private GameObject root;

    private OcTreePoint<OcPointObject> tree;
    private OcPointObject[] points;


    // Start is called before the first frame update
    void Start()
    {
        points = new OcPointObject[itemNum];
        root = new GameObject("Root");
        for (int i = 0; i < itemNum; i++)
        {
            CreateObjects(i);
        }
        CreateOcTree();
        PrintNode();
    }

    void CreateObjects(int i)
    {
        var list = Enum.GetValues(typeof(PrimitiveType));
        int length = list.Length;
        int t = UnityEngine.Random.Range(0, length);
        Transform go = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
        go.name = i.ToString();

        float x = UnityEngine.Random.Range(xRange.x, xRange.y);
        float y = UnityEngine.Random.Range(yRange.x, yRange.y);
        float z = UnityEngine.Random.Range(zRange.x, zRange.y);

        go.SetParent(root.transform, false);
        go.localPosition = new Vector3(x, y, z);

        var m = go.GetComponent<MeshRenderer>();
        m.receiveShadows = false;

        points[i] = new OcPointObject(x, y, z);
    }

    void CreateOcTree()
    {
        if (points == null) return;
        tree = new OcTreePoint<OcPointObject>(new Bounds(xRange.x, xRange.y, yRange.x, yRange.y, zRange.x, zRange.y));
        OcPointObject item;
        for (int i = 0; i < points.Length; i++)
        {
            item = points[i];
            tree.AddObject(item);
        }
    }

    private float m_t = 0;
    private int index = 0;
    private float duration = 2.0f;
    private Color baseColor = new Color(1f, 0f, 0f, 0.5f);
    private void OnDrawGizmos()
    {
        if (tree == null) return;
        var nodes = tree.GetAllNode();
        // 全局
        foreach (var node in nodes) {
            DrawBounds(node.Bounds, new Color(0.5f, 0.5f, 0.5f, 0.5f));                
        }

        // 局部
        int length = nodes.Count;
        while (nodes[index].ObjectContainer.Count == 0) {
            index = index < length - 1 ? index + 1 : 0;
        }            
        DrawBounds(nodes[index].Bounds, baseColor);
        m_t += Time.deltaTime;
        if (m_t <= duration) return;               
        index = index < length - 1 ? index + 1 : 0;
        m_t = 0;
    }

    private void DrawBounds(Bounds bounds, Color color)
    {
        var pos = bounds.Position;
        Gizmos.color = color;
        Gizmos.DrawCube(new Vector3(pos.X, pos.Y, pos.Z), new Vector3(bounds.XSize, bounds.YSize, bounds.ZSize));        
    }

    void PrintNode() {
        var nodes = tree.GetAllNode();
        foreach (var node in nodes) {
            Debug.Log(node.ToString());
        }
    }
}
