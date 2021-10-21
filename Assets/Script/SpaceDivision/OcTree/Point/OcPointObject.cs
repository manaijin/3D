using System.Numerics;

/// <summary>
/// 八叉树点对象
/// </summary>
public class OcPointObject : IOcObject
{
    // 位置
    public Vector3 Position {
        get => position;
        set => position = value;
    }

    protected Vector3 position;

    public OcPointObject() { }

    public OcPointObject(Vector3 pos)
    {
        position = pos;
    }

    public OcPointObject(float x, float y, float z)
    {
        position = new Vector3(x, y, z);
    }

    public virtual bool InSideTreeNode<T>(OcTreePointNode<T> ocTreeNode) where T : IOcObject
    {
        return ocTreeNode.Bounds.Contain(position);
    }

    public virtual bool IntersectTreeNodee<T>(OcTreePointNode<T> ocTreeNode) where T : IOcObject
    {
        return ocTreeNode.Bounds.Contain(position);
    }
}
