using System.Numerics;

/// <summary>
/// AABB包围盒
/// </summary>
public struct Bounds
{
    public Bounds(Vector3 pos, float xSize, float ySize, float zSize)
    {
        position = pos;

        this.xSize = xSize;
        this.ySize = ySize;
        this.zSize = zSize;

        dX = this.xSize / 2;
        dY = this.ySize / 2;
        dZ = this.zSize / 2;

        xRange = new Vector2(position.X - dX, position.X + dX);
        yRange = new Vector2(position.Y - dY, position.Y + dY);
        zRange = new Vector2(position.Z - dZ, position.Z + dZ);
    }

    public Bounds(Vector2 xRange, Vector2 yRange, Vector2 zRange)
    {
        position = new Vector3(xRange.X + xRange.Y, yRange.X + yRange.Y, zRange.X + zRange.Y) / 2;

        xSize = xRange.Y - xRange.X;
        ySize = yRange.Y - yRange.X;
        zSize = zRange.Y - zRange.X;

        dX = xSize / 2;
        dY = ySize / 2;
        dZ = zSize / 2;

        this.xRange = xRange;
        this.yRange = yRange;
        this.zRange = zRange;
    }

    public Bounds(float xMin, float xMax, float yMin, float yMax, float zMin, float zMax) {
        position = new Vector3(xMin + xMax, yMin + yMax, zMin + zMax) / 2;

        xSize = xMax - xMin;
        ySize = yMax - yMin;
        zSize = zMax - zMin;

        dX = xSize / 2;
        dY = ySize / 2;
        dZ = zSize / 2;

        xRange = new Vector2(xMin, xMax);
        yRange = new Vector2(yMin, yMax); 
        zRange = new Vector2(zMin, zMax); 
    }

    // 包围盒位置：中心
    public Vector3 Position
    {
        get => position;
        set
        {
            position = value;
            xRange = new Vector2(position.X - dX, position.X + dX);
            yRange = new Vector2(position.Y - dY, position.Y + dY);
            zRange = new Vector2(position.Z - dZ, position.Z + dZ);
        }
    }

    private Vector3 position;

    // x
    public float XSize
    {
        get => xSize;
        set
        {
            xSize = value;
            dX = xSize / 2;
            xRange = new Vector2(position.X - dX, position.X + dX);
        }
    }
    private float xSize;
    private float dX;

    // y
    public float YSize
    {
        get => ySize;
        set
        {
            ySize = value;
            dY = ySize / 2;
            yRange = new Vector2(position.Y - dY, position.Y + dY);
        }
    }
    private float ySize;
    private float dY;

    // z
    public float ZSize
    {
        get => zSize;
        set
        {
            zSize = value;
            dZ = zSize / 2;
            zRange = new Vector2(position.Z - dZ, position.Z + dZ);
        }
    }
    private float zSize;
    private float dZ;

    // x范围
    public Vector2 XRange
    {
        get => xRange;
        set
        {
            xRange = value;
            xSize = xRange.Y - xRange.Y;
            position.X = (xRange.Y + xRange.Y) / 2;
        }
    }
    private Vector2 xRange;

    // y范围
    public Vector2 YRange
    {
        get => yRange;
        set
        {
            yRange = value;
            ySize = yRange.Y - yRange.X;
            position.Y = (yRange.Y + yRange.Y) / 2;
        }
    }
    private Vector2 yRange;

    // z范围
    public Vector2 ZRange
    {
        get => zRange;
        set
        {
            zRange = value;
            zSize = zRange.Y - zRange.X;
            position.Z = (zRange.Y + zRange.Y) / 2;
        }
    }
    private Vector2 zRange;


    /// <summary>
    /// 是否覆盖点
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public bool Contain(Vector3 point)
    {
        return point.X >= XRange.X && point.X <= XRange.Y && point.Y >= YRange.X && point.Y <= YRange.Y && point.Z >= ZRange.X && point.Z <= ZRange.Y;
    }

    /// <summary>
    /// 是否完全覆盖包围盒
    /// </summary>
    /// <param name="b"></param>
    /// <returns></returns>
    public bool Contain(Bounds b)
    {
        return XRange.X <= b.XRange.X && XRange.Y >= b.XRange.Y && YRange.X <= b.YRange.X && YRange.Y >= b.YRange.Y && ZRange.X <= b.ZRange.X && ZRange.Y >= b.ZRange.Y;
    }

    /// <summary>
    /// 是否与包围盒相交
    /// </summary>
    /// <param name="b"></param>
    /// <returns></returns>
    public bool Intersects(Bounds b)
    {
        return XRange.Y >= b.XRange.X && XRange.X <= b.XRange.Y && YRange.Y >= b.YRange.X && YRange.X <= b.YRange.Y && ZRange.Y >= b.ZRange.X && ZRange.X <= b.ZRange.Y;
    }

    /// <summary>
    /// 八等分包围盒
    /// </summary>
    /// <param name="result"></param>
    /// <returns>是否切分成功</returns>
    public bool Split(out Bounds[] result) {
        result = new Bounds[8];
        // TODO:处理无限切割问题
        if (xSize <= 0 && ySize <= 0 && zSize <= 0) 
            return false;
        
        float newXSize = xSize / 2;
        float newYSize = ySize / 2;
        float newZSize = zSize / 2;

        Vector3 pos = position + new Vector3(-dX, -dY, -dZ) / 2;
        result[0] = new Bounds(pos, newXSize, newYSize, newZSize);

        pos = position + new Vector3(dX, -dY, -dZ) / 2;
        result[1] = new Bounds(pos, newXSize, newYSize, newZSize);

        pos = position + new Vector3(-dX, -dY, dZ) / 2;
        result[2] = new Bounds(pos, newXSize, newYSize, newZSize);

        pos = position + new Vector3(dX, -dY, dZ) / 2;
        result[3] = new Bounds(pos, newXSize, newYSize, newZSize);

        pos = position + new Vector3(-dX, dY, -dZ) / 2;
        result[4] = new Bounds(pos, newXSize, newYSize, newZSize);

        pos = position + new Vector3(dX, dY, -dZ) / 2;
        result[5] = new Bounds(pos, newXSize, newYSize, newZSize);

        pos = position + new Vector3(-dX, dY, dZ) / 2;
        result[6] = new Bounds(pos, newXSize, newYSize, newZSize);

        pos = position + new Vector3(dX, dY, dZ) / 2;
        result[7] = new Bounds(pos, newXSize, newYSize, newZSize);

        return true;
    }
}
