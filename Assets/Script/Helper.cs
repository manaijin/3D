using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class Helper
{
    /// <summary>
    /// 画线（原点为屏幕中心）
    /// </summary>
    /// <param name="start">起点坐标</param>
    /// <param name="end">终点坐标</param>
    /// <param name="c">颜色</param>
    public static void DrawLine(Vector3 start, Vector3 end, Color c)
    {
        DrawLine(start.x, start.y, end.x, end.y, c);
    }

    /// <summary>
    /// 画线（原点为屏幕中心）
    /// </summary>
    /// <param name="startX">起点坐标X</param>
    /// <param name="startY">起点坐标Y</param>
    /// <param name="endX">终点坐标X</param>
    /// <param name="endY">终点坐标Y</param>
    /// <param name="c">颜色</param>
    public static void DrawLine(float startX, float startY, float endX, float endY, Color c)
    {
        GL.Color(c);
        var v = GetVerticalVector(endX - startX, endY - startY);
        v = v.normalized * 0.005f;
        // gl接口绘制有间隙，多重绘制增加宽度
        for (int i = 0; i < 4; i++)
        {
            GL.Vertex3(startX + i * v.x, startY + i * v.y, 0);
            GL.Vertex3(endX + i * v.x, endY + i * v.y, 0);
        }
    }


    /// <summary>
    /// 获取二维垂直向量
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static Vector2 GetVerticalVector(float x, float y)
    {
        float vX = y / (x - y);
        float vY = 1 - vX;
        return new UnityEngine.Vector2(vX, vY);
    }
}
