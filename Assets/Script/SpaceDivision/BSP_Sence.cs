using System.Collections.Generic;
using System.Numerics;
using System.Text;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class BSP_Sence : MonoBehaviour
{
    [Tooltip("是否自动生成线段，false需要手动输入数据")]
    public bool isAutoGenerateLines = true;
    public UnityEngine.Vector2 xRange = new UnityEngine.Vector2(-10, 10);
    public UnityEngine.Vector2 yRange = new UnityEngine.Vector2(-10, 10);
    public List<Line2D> lines;


    void Start()
    {
        GenerateLines();
        PrintLines();
    }

    private void Update()
    {
        if (lines == null) return;
        foreach (Line2D line in lines)
        {
            var strat = line.startPoint;
            var end = line.endPoint;
            Debug.DrawLine(new UnityEngine.Vector3(strat.X, strat.Y), new UnityEngine.Vector3(end.X, end.Y));
        }        
    }

    /// <summary>
    /// 生成空间分割线段（不能交叉）
    /// </summary>
    void GenerateLines()
    {
        if (!isAutoGenerateLines) return;
        lines = new List<Line2D>();
        int num = Random.Range(3, 10);
        for (int i = 0; i < num; i++)
        {
            Line2D line;
            do
            {
                line = CreateLine();
            } while (IsLineCrossCurrent(line));

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
        foreach (var item in lines)
        {
            if (Line2D.IsLineCross(item, line))
                return true;
        }
        return false;
    }


    Line2D CreateLine()
    {
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
