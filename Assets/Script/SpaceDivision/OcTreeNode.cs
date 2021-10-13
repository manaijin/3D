using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 八叉树节点
/// </summary>
class OcTreeNode<T> : IOcTreeNode<T>
{
    public Bounds Bounds { get; set; }

    private OcTreeNode<T>[] childNode;

    private List<T> objectContainer;

    public void InsertObject(T obj)
    {
        throw new NotImplementedException();
    }

    public void OnTrigger(IDetector detector)
    {
        throw new NotImplementedException();
    }
}

public interface IOcTreeNode<T>
{
    /// <summary>
    /// 插入对象
    /// </summary>
    /// <param name="obj"></param>
    public void InsertObject(T obj);

    /// <summary>
    /// 检测区域触
    /// </summary>
    /// <param name="detector"></param>
    public void OnTrigger(IDetector detector);
}

public interface IDetector
{
    /// <summary>
    /// 探测结果
    /// </summary>
    /// <param name="bounds">包围盒</param>
    /// <returns></returns>
    bool IsDetected(Bounds bounds);

    /// <summary>
    /// 触发器位置
    /// </summary>
    Vector3 Position { get; }
}