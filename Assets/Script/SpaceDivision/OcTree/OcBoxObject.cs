/// <summary>
/// 八叉树包围盒对象
/// </summary>
public class OcBoxObject : OcPointObject
{
    public OcBoxObject(Bounds bounds)
    {
        this.bounds = bounds;
        position = bounds.Position;
    }

    public Bounds Bounds {
        get => bounds;
    }
    public Bounds bounds;

    public override bool InSideTreeNode<T>(IOcTreeNode<T> ocTreeNode)
    {
        return (ocTreeNode as OcTreePointNode<T>).Bounds.Contain(Bounds);
    }

    public override bool IntersectTreeNodee<T>(IOcTreeNode<T> ocTreeNode) 
    {
        return (ocTreeNode as OcTreePointNode<T>).Bounds.Intersects(Bounds);
    }
}
