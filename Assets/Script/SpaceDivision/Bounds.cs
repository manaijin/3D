using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Box包围盒
/// </summary>
public struct Bounds
{
    // 包围盒位置：中心
    public Vector3 Position;
    // x范围
    public Vector2 xSize;
    // y范围
    public Vector2 ySize;
    // z范围
    public Vector2 zSize;
}
