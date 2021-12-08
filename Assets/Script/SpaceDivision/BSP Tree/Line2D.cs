using System.Numerics;

public class Line2D : IBSPNode
{
    public Vector2 startPoint;
    public Vector2 endPoint;

    /// <summary>
    /// 两条直线是否相交
    /// </summary>
    /// <param name="line1"></param>
    /// <param name="line2"></param>
    /// <returns></returns>
    public static bool IsLineCross(Line2D line1, Line2D line2) 
    {
        return IsPointsCrossLine(line1, line2.startPoint, line2.endPoint) && IsPointsCrossLine(line2, line1.startPoint, line1.endPoint) ;
    }

    /// <summary>
    /// 判断平面两点是否在直线两侧
    /// </summary>
    /// <param name="line"></param>
    /// <param name="point1"></param>
    /// <param name="point2"></param>
    /// <returns></returns>
    private static bool IsPointsCrossLine(Line2D line, Vector2 point1, Vector2 point2) {
        var r1 = line.PointSide(point1);
        var r2 = line.PointSide(point2);
        return r1 + r2 == 0 && r1 != 0;
    }

    /// <summary>
    /// 计算点与直线的关系
    /// </summary>
    /// <param name="point"></param>
    /// <returns>-1：直线一侧</returns>
    /// <returns>0：直线上</returns>
    /// <returns>1：直线另一侧</returns>
    private int PointSide(Vector2 point) 
    {
        float f = (endPoint.Y - startPoint.Y) * (point.X - startPoint.X) - (endPoint.X - startPoint.X) * (point.Y - startPoint.Y);
        int restlt;
        if (f < 0)
            restlt = -1;
        else if (f == 0)
            restlt = 0;
        else
            restlt = 1;
        return restlt;
    }

    public override string ToString()
    {
        return string.Format("起点：{0}， 终点：{1}", startPoint.ToString(), endPoint.ToString());
    }
}
