using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 八叉树空间节点抽象类
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class OcTreeBaseNode<T> : IOcTreeNode<T> where T : IOcObject
{
    /// <summary>
    /// 空间子节点
    /// </summary>
    protected OcTreePointNode<T>[] childNode;

    /// <summary>
    /// 对象列表
    /// </summary>
    public List<T> ObjectContainer;

    /// <summary>
    /// 空间对象限制数量（可超出）
    /// </summary>
    public int LimitNum
    {
        get => limitNum;
        set
        {
            if (value < 1)
                limitNum = 1;
        }
    }
    private int limitNum = 2;

    /// <summary>
    /// 空间小尺寸（每个维度单独判断）
    /// 若单个维度小于等于0，则不判断
    /// </summary>
    public Vector3 MinSize = Vector3.Zero;

    public abstract void AddObject(T obj);

    public abstract void FindRelatedObjects(T obj, ref List<T> resoult);

    public virtual void GenerateChildNode()
    {
        if (!TrySplitNode(out var childBounds)) return;

        childNode = new OcTreePointNode<T>[8];
        for (int i = 0; i < 8; i++)
        {
            childNode[i] = new OcTreePointNode<T>(childBounds[i]);
        }

        TryMoveContainedObjectToChildNode();
    }

    /// <summary>
    /// 尝试八等分节点
    /// </summary>
    /// <param name="childBounds">划分后的8个叶子节点</param>
    /// <returns>是否划分成功</returns>
    protected bool TrySplitNode(out Bounds[] childBounds)
    {
        childBounds = null;

        // 划分节点是否满足最小空间尺寸
        if (MinSize.X > 0 && (MinSize.X * 2 > Bounds.XSize))
            return false;

        if (MinSize.Y > 0 && (MinSize.Y * 2 > Bounds.YSize))
            return false;

        if (MinSize.Z > 0 && (MinSize.Z * 2 > Bounds.ZSize))
            return false;

        // 划分
        return Bounds.Split(out childBounds);
    }

    /// <summary>
    /// 尝试将当前节点包含的对象移动到子对象中
    /// </summary>
    protected void TryMoveContainedObjectToChildNode()
    {
        if (ObjectContainer == null) return;
        int length = ObjectContainer.Count;
        for (int i = length - 1; i >= 0; i--)
        {
            var obj = ObjectContainer[i];
            foreach (var node in childNode)
            {
                if (obj.InSideTreeNode(node))
                {
                    ObjectContainer.RemoveAt(i);
                    node.AddObject(obj);
                    break;
                }
            }
        }
    }

    public virtual bool RemoveObject(T obj)
    {
        if (!obj.InSideTreeNode(this)) return false;

        // 从子节点中删除
        if (childNode != null)
        {
            foreach (var child in childNode)
            {
                if (child.RemoveObject(obj))
                    return true;
            }
        }
        // 从当前节点删除
        foreach (var item in ObjectContainer)
        {
            if (item.Equals(obj))
                return true;
        }

        return false;
    }

    private static readonly string s2 = "\t场景对象：";

    public Bounds Bounds { get; set; }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder(16);
        sb.Append(Bounds.ToString());
        sb.Append("\n");
        for (int i = 0; i < ObjectContainer.Count; i++)
        {
            sb.Append(s2);
            sb.Append(ObjectContainer[i].Position.ToString());
            sb.Append("\n");
        }
        return sb.ToString();
    }
}
