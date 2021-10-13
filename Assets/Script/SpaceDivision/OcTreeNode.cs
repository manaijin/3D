using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 八叉树节点
/// </summary>
class OcTreeNode
{

}

public interface IOcTreeNode
{
    // Property declaration:
    public Bounds bounds
    {
        get;
        set;
    }
}