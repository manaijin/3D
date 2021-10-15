using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 八叉树包围盒对象
/// </summary>
class OcBoxObject : OcPointObject
{
    public Bounds Bounds { get; set; }

    public override bool InSideTreeNode<T>(OcTreeNode<T> ocTreeNode)
    {
        return ocTreeNode.Bounds.Contain(Bounds);
    }

    public override bool IntersectTreeNodee<T>(OcTreeNode<T> ocTreeNode) 
    {
        return ocTreeNode.Bounds.Intersects(Bounds);
    }
}
