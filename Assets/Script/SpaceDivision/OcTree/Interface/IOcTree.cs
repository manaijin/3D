using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

interface IOcTree<T> where T : IOcObject
{
    /// <summary>
    /// 插入对象
    /// </summary>
    /// <param name="obj"></param>
    public void InsertObject(T obj);

    /// <summary>
    /// 查找关联对象
    /// </summary>
    /// <param name="obj"></param>
    public List<T> FindRelatedObjects(T obj);
}
