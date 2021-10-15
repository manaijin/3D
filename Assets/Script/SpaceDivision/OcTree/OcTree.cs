using System.Collections.Generic;
/// <summary>
/// 八叉树
/// </summary>
class OcTree<T> : IOcTree<T> where T: IOcObject
{
    public Bounds Bounds => bounds;
    private Bounds bounds;

    private OcTreeNode<T> rootNode;

    public OcTree(Bounds b)
    {
        bounds = b;
        rootNode = new OcTreeNode<T>(bounds);
    }

    public void InsertObject(T obj) 
    {
        rootNode.InsertObject(obj);
    }

    public List<T> FindRelatedObjects(T obj)
    {
        List<T> result = new List<T>();
        rootNode.FindRelatedObjects(obj, ref result);
        return result;
    }

    public List<OcTreeNode<T>> GetAllNode() {
        List<OcTreeNode<T>> result = new List<OcTreeNode<T>>();
        rootNode.GetAllNode(ref result);
        return result;
    }

    public void Splite(T[] objs)
    {
        throw new System.NotImplementedException();
    }
}


