using System;
using System.Numerics;

[Serializable]
public class Line2D :ICloneable
{
    public Vector2 startPoint;
    public Vector2 endPoint;

    /// <summary>
    /// 两条线段是否相交
    /// </summary>
    /// <param name="segment1">线段1</param>
    /// <param name="segment2">线段2</param>
    /// <returns></returns>
    public static bool IsLineCross(Line2D segment1, Line2D segment2)
    {
        return IsPointsCrossLine(segment1, segment2.startPoint, segment2.endPoint) && IsPointsCrossLine(segment2, segment1.startPoint, segment1.endPoint);
    }

    /// <summary>
    /// 获取直线与线段交点
    /// </summary>
    /// <param name="line"></param>
    /// <param name="segment"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    public static bool GetCrossPoint(Line2D line, Line2D segment, out Vector2 point)
    {
        if (IsPointsCrossLine(line, segment.startPoint, segment.endPoint))
        {
            Vector2 p1 = line.startPoint, p2 = line.endPoint, p3 = segment.startPoint, p4 = segment.endPoint;
            float x = ((p3.X - p4.X) * (p2.X * p1.Y - p1.X * p2.Y) - (p1.X - p2.X) * (p4.X * p3.Y - p3.X * p4.Y)) / ((p3.X - p4.X) * (p1.Y - p2.Y) - (p1.X - p2.X) * (p3.Y - p4.Y));
            float y = ((p3.Y - p4.Y) * (p2.Y * p1.X - p1.Y * p2.X) - (p1.Y - p2.Y) * (p4.Y * p3.X - p3.Y * p4.X)) / ((p3.Y - p4.Y) * (p1.X - p2.X) - (p1.Y - p2.Y) * (p3.X - p4.X));
            point = new Vector2(x, y);
            return true;
        }
        else
        {
            point = default;
            return false;
        }
    }

    /// <summary>
    /// 判断平面两点是否在直线两侧
    /// </summary>
    /// <param name="segment"></param>
    /// <param name="point1"></param>
    /// <param name="point2"></param>
    /// <returns></returns>
    public static bool IsPointsCrossLine(Line2D segment, Vector2 point1, Vector2 point2)
    {
        var r1 = segment.PointSide(point1);
        var r2 = segment.PointSide(point2);
        return r1 + r2 == 0 && r1 != 0;
    }

    /// <summary>
    /// 计算点与直线的关系
    /// </summary>
    /// <param name="point"></param>
    /// <returns>-1：直线一侧</returns>
    /// <returns>0：直线上</returns>
    /// <returns>1：直线另一侧</returns>
    public int PointSide(Vector2 point)
    {
        float f = (endPoint.Y - startPoint.Y) * (point.X - startPoint.X) - (endPoint.X - startPoint.X) * (point.Y - startPoint.Y);
        float d = 0.001f;
        int restlt;
        if (f < -d)
            restlt = -1;
        else if (f > d)
            restlt = 1;
        else
            restlt = 0;
        return restlt;
    }

    public override string ToString()
    {
        return string.Format("起点：{0}， 终点：{1}", startPoint.ToString(), endPoint.ToString());
    }

    public object Clone()
    {
        return new Line2D() { startPoint = this.startPoint, endPoint = this.endPoint };
    }
}
