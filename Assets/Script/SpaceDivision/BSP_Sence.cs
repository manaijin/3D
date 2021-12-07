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
    public Material mat;
    public List<Line2D> lines;

    private UnityEngine.Vector3[] ViewRange;
    void Start()
    {
        ViewRange = new UnityEngine.Vector3[4];
        ViewRange[0] = new UnityEngine.Vector3(xRange.x, yRange.x);
        ViewRange[1] = new UnityEngine.Vector3(xRange.x, yRange.y);
        ViewRange[2] = new UnityEngine.Vector3(xRange.y, yRange.y);
        ViewRange[3] = new UnityEngine.Vector3(xRange.y, yRange.x);
        GenerateLines();
        PrintLines();
    }

    void Update()
    {

    }

    void OnGUI()
    {
        if (lines == null) return;
        if (!mat) return;
        mat.SetPass(0);
        GL.PushMatrix();
        foreach (Line2D line in lines)
        {
            var strat = line.startPoint;
            var end = line.endPoint;
            DrawLine(new UnityEngine.Vector3(strat.X, strat.Y), new UnityEngine.Vector3(end.X, end.Y), Color.black);
        }
        DrawLine(ViewRange[0], ViewRange[1], Color.white);
        DrawLine(ViewRange[1], ViewRange[2], Color.white);
        DrawLine(ViewRange[2], ViewRange[3], Color.white);
        DrawLine(ViewRange[3], ViewRange[0], Color.white);
        GL.PopMatrix();
    }

    void DrawLine(UnityEngine.Vector3 start, UnityEngine.Vector3 end, Color c)
    {
        GL.Begin(GL.LINES);
        GL.Color(c);
        GL.Vertex(start);
        GL.Vertex(end);
        GL.End();
    }

    /// <summary>
    /// 生成空间分割线段（不能交叉）
    /// </summary>
    void GenerateLines()
    {
        if (!isAutoGenerateLines) return;
        lines = new List<Line2D>();

        for (int i = 0; i < pointNum - 1; i++)
        {
            Line2D line;
            do
            {
                line = CreateLine();
            } while (line != null && IsLineCrossCurrent(line));

            if (line == null) return;
            lines.Add(line);
        }

        Line2D line1;
        Line2D line2;
        do
        {
            line1 = CreateLine();
            if (line1 == null) return;
            line2 = new Line2D()
            {
                startPoint = line1.endPoint,
                endPoint = lines[0].startPoint
            };
        } while (IsLineCrossCurrent(line1) || IsLineCrossCurrent(line2) && line1 != null && line2 != null);
        if (line2 == null) return;
        lines.Add(line1);
        lines.Add(line2);
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
