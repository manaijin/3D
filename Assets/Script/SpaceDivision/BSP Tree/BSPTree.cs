using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BSPTree : IBSPTree
{
    public BSPNode<Line2D> RootNode { get => rootNode; }
    private BSPNode<Line2D> rootNode;

    public void CreateTree(IList<Line2D> lines, BSPNode<Line2D> p = null)
    {
        if (lines.Count == 0) return;

        if (rootNode == null)
        {
            rootNode = new BSPNode<Line2D>(lines[0]);
            p = rootNode;
        }

        Line2D line = lines[0];

        // 分割空间
        List<Line2D> leftNodes = new List<Line2D>();
        List<Line2D> rightNodes = new List<Line2D>();

        foreach (var segment in lines)
        {
            AddLine(leftNodes, rightNodes, line, segment);
        }

        // 迭代
        p.LeftNode = leftNodes.Count > 0 ? new BSPNode<Line2D>(leftNodes[0]) : null;
        p.RightNode = rightNodes.Count > 0 ? new BSPNode<Line2D>(rightNodes[0]) : null;
        CreateTree(leftNodes, p.LeftNode);
        CreateTree(rightNodes, p.RightNode);
    }

    private void AddLine(List<Line2D> leftNode, List<Line2D> rightNode, Line2D line, Line2D segment)
    {
        var s1 = line.PointSide(segment.startPoint);
        var s2 = line.PointSide(segment.endPoint);
        if (s1 + s2 < 0)
        {
            leftNode.Add(segment);
        }

        if (s1 + s2 > 0)
        {
            rightNode.Add(segment);
        }

        if (s1 + s2 == 0)
        {
            // 相交
            if (Line2D.GetCrossPoint(line, segment, out var newP))
            {
                var newLine1 = new Line2D() { startPoint = segment.startPoint, endPoint = newP };
                var newLine2 = new Line2D() { startPoint = newP, endPoint = segment.endPoint };
                line.endPoint = newP;
                AddLine(leftNode, rightNode, line, newLine1);
                AddLine(leftNode, rightNode, line, newLine2);
            }
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("BSP树：\n\r");
        ENode(rootNode, (BSPNode<Line2D> node)=> {
            sb.Append(node.ToString());
            sb.Append('\n');
        });
        return sb.ToString();
    }

    public void ENode(BSPNode<Line2D> node, Action<BSPNode<Line2D>> c)
    {
        if (node == null) return;
        if (c != null)
            c(node);
        ENode(node.LeftNode, c);
        ENode(node.RightNode, c);
    }
}
