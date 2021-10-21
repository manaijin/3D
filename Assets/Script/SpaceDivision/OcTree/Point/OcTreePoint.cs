using System.Collections.Generic;
/// <summary>
/// 八叉树（空间对象为point）
/// </summary>
class OcTreePoint<T> : IOcTree<T> where T: IOcObject
{
    public Bounds Bounds => bounds;
    private Bounds bounds;

    private OcTreePointNode<T> rootNode;

    public OcTreePoint(Bounds b)
    {
        bounds = b;
        rootNode = new OcTreePointNode<T>(bounds);
    }

    public void AddObject(T obj) 
    {
        rootNode.AddObject(obj);
    }

    public List<T> FindRelatedObjects(T obj)
    {
        List<T> result = new List<T>();
        rootNode.FindRelatedObjects(obj, ref result);
        return result;
    }

    public List<OcTreePointNode<T>> GetAllNode() {
        List<OcTreePointNode<T>> result = new List<OcTreePointNode<T>>();
        rootNode.GetAllNode(ref result);
        return result;
    }

    public void RemoveObject(T obj)
    {
        rootNode.RemoveObject(obj);
    }
}


