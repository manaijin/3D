using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public abstract void AddObject(T obj);

    public abstract void FindRelatedObjects(T obj, ref List<T> resoult);

    public virtual void GenerateChildNode()
    {
        // 划分子节点
        if (!this.Bounds.Split(out var childBounds)) return;

        childNode = new OcTreePointNode<T>[8];
        for (int i = 0; i < 8; i++)
        {
            childNode[i] = new OcTreePointNode<T>(childBounds[i]);
        }

        // 尝试将已有对象放入子节点中
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

    private string s1 = "范围:";
    private string s2 = "\t场景对象：";

    public Bounds Bounds { get ; set ; }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder(16);
        sb.Append(s1);
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
