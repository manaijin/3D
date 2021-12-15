using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BSPNode<T> : IBSPNode
{
    public BSPNode<T> LeftNode;
    public BSPNode<T> RightNode;
    public T data;

    public BSPNode(T node)
    {
        data = node;
    }

    public override string ToString()
    {
        return data.ToString();
    }
}
