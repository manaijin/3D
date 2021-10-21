using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

interface IOcTree<T> where T : IOcObject
{
    /// <summary>
    /// 添加对象
    /// </summary>
    /// <param name="obj"></param>
    public void AddObject(T obj);

    /// <summary>
    /// 查找关联对象
    /// </summary>
    /// <param name="obj"></param>
    public List<T> FindRelatedObjects(T obj);


    /// <summary>
    /// 移除空间对象
    /// </summary>
    /// <param name="obj">空间对象</param>
    public void RemoveObject(T obj);
}
