using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

/// <summary>
/// 八叉树节点
/// </summary>
public class OcTreeNode<T> : IOcTreeNode<T> where T : IOcObject
{
    /// <summary>
    /// 空间区域
    /// </summary>
    public Bounds Bounds { get; set; }

    // 空间子节点
    public OcTreeNode<T>[] ChildNode
    {
        get
        {
            if (childNode == null)
            {
                GenerateChildNode();
            }
            return childNode;
        }
    }
    private OcTreeNode<T>[] childNode;

    public int maxNum = -1;
    public Vector3 minSize = new Vector3(-1, -1, -1);

    // 对象列表
    private List<T> objectContainer;

    public OcTreeNode() { }

    public OcTreeNode(Bounds b)
    {
        Bounds = b;
        objectContainer = new List<T>();
    }

    public OcTreeNode(Bounds b, params T[] objs) : this(b)
    {
        for (int i = 0; i < objs.Length; i++)
        {
            InsertObject(objs[i]);
        }
    }

    public void InsertObject(T obj)
    {
        if (!obj.InSideTreeNode(this)) return;
        if (ChildNode == null) return;
        foreach (var node in ChildNode)
        {
            if (obj.InSideTreeNode(node))
            {
                node.InsertObject(obj);
                return;
            }
        }

        objectContainer.Add(obj);
    }

    public void GenerateChildNode()
    {
        // TODO:对象池处理对象创建        
        if (!Bounds.Split(out var childBounds)) return;
        childNode = new OcTreeNode<T>[8];
        for (int i = 0; i < 8; i++)
        {
            childNode[i] = new OcTreeNode<T>(childBounds[i]);
        }
    }

    public void FindRelatedObjects(T obj, ref List<T> resoult)
    {
        if (!obj.IntersectTreeNodee(this)) return;
        resoult.AddRange(objectContainer);

        foreach (var node in ChildNode)
        {
            if (obj.IntersectTreeNodee(this))
            {
                node.InsertObject(obj);
            }
        }
    }

    public void GetAllNode(ref List<OcTreeNode<T>> result) {
        result.Add(this);
        if(ChildNode != null)
            result.AddRange(ChildNode);
    }

    public override string ToString() {
        StringBuilder sb = new StringBuilder();
        sb.Append("范围:");
        sb.Append(Bounds.ToString());
        sb.Append("\n");
        for (int i = 0; i < objectContainer.Count; i++) {
            sb.Append("\t场景对象：");
            sb.Append(objectContainer[i].Position.ToString());
            sb.Append("\n");
        }
        return sb.ToString();
    }
}