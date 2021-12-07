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

    public UnityEngine.Vector2 xRange = new UnityEngine.Vector2(-10, 10);
    public UnityEngine.Vector2 yRange = new UnityEngine.Vector2(-10, 10);

    [Tooltip("画线材质")]
    public Material lineMaterial;

    private List<Line2D> lines;
    private UnityEngine.Vector3[] ViewPoint;

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
    }

    void Update()
    {
        //if (lines == null) return;
        //foreach (Line2D line in lines)
        //{
        //    var strat = line.startPoint;
        //    var end = line.endPoint;
        //    Debug.DrawLine(new UnityEngine.Vector3(strat.X, strat.Y), new UnityEngine.Vector3(end.X, end.Y), Color.black);
        //}
        //// 边框
        //Debug.DrawLine(ViewPoint[0], ViewPoint[1], Color.white);
        //Debug.DrawLine(ViewPoint[1], ViewPoint[2], Color.white);
        //Debug.DrawLine(ViewPoint[2], ViewPoint[3], Color.white);
        //Debug.DrawLine(ViewPoint[3], ViewPoint[0], Color.white);
    }

    void OnRenderObject()
    {
        if (lines == null) return;
        if (!lineMaterial) return;
        GL.PushMatrix();

        lineMaterial.SetPass(0);
        GL.Begin(GL.LINES);
        // 线段
        foreach (Line2D line in lines)
        {
            var strat = line.startPoint;
            var end = line.endPoint;
            DrawLine(strat.X, strat.Y, end.X, end.Y, Color.black);
        }

        // 边框        
        DrawLine(ViewPoint[0], ViewPoint[1], Color.white);
        DrawLine(ViewPoint[1], ViewPoint[2], Color.white);
        DrawLine(ViewPoint[2], ViewPoint[3], Color.white);
        DrawLine(ViewPoint[3], ViewPoint[0], Color.white);
        GL.End();

        GL.PopMatrix();
    }


    public void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            lineMaterial.SetInt("_ZWrite", 1);
        }
    }

    private UnityEngine.Vector3 off = new UnityEngine.Vector3(0.01f, 0.01f, 0.01f);
    void DrawLine(UnityEngine.Vector3 start, UnityEngine.Vector3 end, Color c)
    {
        GL.Color(c);
        // gl接口绘制有间隙，多重绘制增加宽度
        for (int i = 0; i < 2; i++)
        {
            GL.Vertex(start + i * off);
            GL.Vertex(end + i * off);
        }
    }

    void DrawLine(float startX, float startY, float endX, float endY, Color c)
    {
        GL.Color(c);
        // gl接口绘制有间隙，多重绘制增加宽度
        for (int i = 0; i < 2; i++)
        {
            GL.Vertex3(startX + i * off.x, startY + i * off.y, 0);
            GL.Vertex3(endX + i * off.x, endY + i * off.y, 0);
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
