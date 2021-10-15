using System;
using System.Collections.Generic;

/// <summary>
/// 八叉树节点
/// </summary>
class OcTreeNode<T> : IOcTreeNode<T> where T : IOcObject
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

    // 对象列表
    private List<T> objectContainer;

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

        foreach (var node in ChildNode)
        {
            if (obj.InSideTreeNode(this))
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
        childNode = new OcTreeNode<T>[8];
        var childBounds = Bounds.Split();
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
}