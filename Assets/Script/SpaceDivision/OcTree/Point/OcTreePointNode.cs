using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

/// <summary>
/// 八叉树节点（空间对象为point）
/// </summary>
public class OcTreePointNode<T> : OcTreeBaseNode<T> where T : IOcObject
{
    // 最小尺寸
    //public Vector3 MinSize = new Vector3(-1, -1, -1);

    // 最大数量
    public int MaxNum = 2;

    public OcTreePointNode() { }

    public OcTreePointNode(Bounds b)
    {
        Bounds = b;
        ObjectContainer = new List<T>();
    }

    public OcTreePointNode(Bounds b, params T[] objs) : this(b)
    {
        for (int i = 0; i < objs.Length; i++)
        {
            AddObject(objs[i]);
        }
    }

    public override void AddObject(T obj)
    {
        if (!obj.InSideTreeNode(this)) return;
        if (childNode == null && ObjectContainer.Count < MaxNum)
        {
            ObjectContainer.Add(obj);
        }
        else
        {
            if (childNode == null)
            {
                GenerateChildNode();
            }

            foreach (var node in childNode)
            {
                if (obj.InSideTreeNode(node))
                {
                    node.AddObject(obj);
                    return;
                }
            }
        }
    }

    public override void FindRelatedObjects(T obj, ref List<T> resoult)
    {
        if (!obj.IntersectTreeNodee(this)) return;
        resoult.AddRange(ObjectContainer);

        foreach (var node in childNode)
        {
            node.FindRelatedObjects(obj, ref resoult);
        }
    }

    public void GetAllNode(ref List<OcTreePointNode<T>> result)
    {
        result.Add(this);
        if (childNode != null) 
        {
            foreach (var node in childNode) {
                node.GetAllNode(ref result);
            }
        }           
    }
}