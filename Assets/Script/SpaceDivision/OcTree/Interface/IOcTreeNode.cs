using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IOcTreeNode<T> where T : IOcObject
{
    /// <summary>
    /// 添加空间对象
    /// </summary>
    /// <param name="obj">空间对象</param>
    public void AddObject(T obj);

    /// <summary>
    /// 生成子节点
    /// </summary>
    public void GenerateChildNode();

    /// <summary>
    /// 查找关联对象
    /// </summary>
    /// <param name="obj">对象数据</param>
    /// <param name="resoult">结果</param>
    /// <returns></returns>
    public void FindRelatedObjects(T obj, ref List<T> resoult);

    /// <summary>
    /// 移除空间对象
    /// </summary>
    /// <param name="obj">空间对象</param>
    /// <returns>是否移除成功</returns>
    public bool RemoveObject(T obj);
}
