using System.Collections.Generic;
using System.Numerics;
using System.Text;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class BSP_Sence : MonoBehaviour
{
    [Tooltip("是否自动生成线段，false需要手动输入数据")]
    public bool isAutoGenerateLines = true;

    [Tooltip("自动生成的顶点数量")]
    [Range(4, 50)]
    public int pointNum = 5;

    // 顶点范围
    public UnityEngine.Vector2 xRange = new UnityEngine.Vector2(-10, 10);
    public UnityEngine.Vector2 yRange = new UnityEngine.Vector2(-10, 10);

    [Tooltip("画线材质，默认创建")]
    public Material lineMaterial;

    // 随机线段
    private List<Line2D> lines;
    // 视图四个顶点
    private UnityEngine.Vector3[] ViewPoint;

    private BSPTree tree;

    void Start()
    {
        ViewPoint = new UnityEngine.Vector3[4];
        ViewPoint[0] = new UnityEngine.Vector3(xRange.x, yRange.x);
        ViewPoint[1] = new UnityEngine.Vector3(xRange.x, yRange.y);
        ViewPoint[2] = new UnityEngine.Vector3(xRange.y, yRange.y);
        ViewPoint[3] = new UnityEngine.Vector3(xRange.y, yRange.x);
        CreateLineMaterial();

        GenerateLines();
        PrintLines();

        tree = new BSPTree();
        var lines2 = new List<Line2D>();
        foreach (var item in lines)
        {
            lines2.Add(item.Clone() as Line2D);
        }
        tree.CreateTree(lines2);
        print(tree.ToString());
    }

    void OnRenderObject()
    {
        if (lines == null) return;
        if (!lineMaterial) return;
        lineMaterial.SetPass(0);
        GL.PushMatrix();
        GL.Begin(GL.LINES);

        // BSP树
        tree.ENode(tree.RootNode, (BSPNode<Line2D> node) =>
        {
            var strat = node.data.startPoint;
            var end = node.data.endPoint;
            Helper.DrawLine(strat.X, strat.Y, end.X, end.Y, Color.red);
        });

        // 原始数据
        foreach (Line2D line in lines)
        {
            var strat = line.startPoint;
            var end = line.endPoint;
            Helper.DrawLine(strat.X, strat.Y, end.X, end.Y, Color.black);
        }

        // 边框        
        Helper.DrawLine(ViewPoint[0], ViewPoint[1], Color.white);
        Helper.DrawLine(ViewPoint[1], ViewPoint[2], Color.white);
        Helper.DrawLine(ViewPoint[2], ViewPoint[3], Color.white);
        Helper.DrawLine(ViewPoint[3], ViewPoint[0], Color.white);
        GL.End();
        GL.PopMatrix();
    }

    public void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader)
            {
                hideFlags = HideFlags.HideAndDontSave
            };
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }


    /// <summary>
    /// 生成空间分割线段（不能交叉）
    /// </summary>
    void GenerateLines()
    {
        if (!isAutoGenerateLines) return;
        lines = new List<Line2D>();

        for (int i = 0; i < pointNum; i++)
        {
            Line2D line;
            do
            {
                line = CreateLine();
            } while (line != null && IsLineCrossCurrent(line));

            if (line == null) return;
            lines.Add(line);
        }
    }


    void PrintLines()
    {
        StringBuilder sb = new StringBuilder();
        int i = 0;
        foreach (var item in lines)
        {
            sb.Append("point");
            sb.Append(i++);
            sb.Append(" ");
            sb.Append(item.ToString());
            sb.Append('\n');
        }
        Debug.Log(sb.ToString());
    }

    bool IsLineCrossCurrent(Line2D line)
    {
        if (line == null) return false;
        foreach (var item in lines)
        {
            if (Line2D.IsLineCross(item, line))
                return true;
        }
        return false;
    }

    int numflag = 0;
    Line2D CreateLine()
    {
        numflag++;
        if (numflag > pointNum * 20)
        {
            Debug.LogError("随机生成Line次数过多");
            return null;
        }

        float x1 = Random.Range(xRange.x, xRange.y);
        float y1 = Random.Range(yRange.x, yRange.y);
        Line2D line;
        if (lines.Count == 0)
        {
            float x2 = Random.Range(xRange.x, xRange.y);
            float y2 = Random.Range(yRange.x, yRange.y);
            line = new Line2D()
            {
                startPoint = new Vector2(x1, y1),
                endPoint = new Vector2(x2, y2),
            };
        }
        else
        {
            line = new Line2D()
            {
                startPoint = lines[lines.Count - 1].endPoint,
                endPoint = new Vector2(x1, y1),
            };
        }
        return line;
    }
}
