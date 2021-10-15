using System.Numerics;
/// <summary>
/// 八叉树场景空间对象
/// </summary>
public interface IOcObject
{
    public Vector3 Position { get; set; }

    /// <summary>
    /// 是否处于树节点空间内部
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ocTreeNode">树节点</param>
    /// <returns></returns>
    public bool InSideTreeNode<T>(OcTreeNode<T> ocTreeNode) where T : IOcObject;

    /// <summary>
    /// 是否与空间节点相交
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ocTreeNode">树节点</param>
    /// <returns></returns>
    public bool IntersectTreeNodee<T>(OcTreeNode<T> ocTreeNode) where T : IOcObject;
}
