using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 八叉树点对象
/// </summary>
class OcPointObject : IOcObject
{
    // 位置
    public Vector3 Position {
        get => position;
    }
    protected Vector3 position;



    public virtual bool InSideTreeNode<T>(OcTreeNode<T> ocTreeNode) where T : IOcObject
    {
        return ocTreeNode.Bounds.Contain(position);
    }

    public virtual bool IntersectTreeNodee<T>(OcTreeNode<T> ocTreeNode) where T : IOcObject
    {
        return ocTreeNode.Bounds.Contain(position);
    }
}
