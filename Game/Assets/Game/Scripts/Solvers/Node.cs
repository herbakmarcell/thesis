using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Node
{
    public Node(State state, Node parent = null)
    {
        State = state;
        Depth = 0;
        Parent = parent;
        Children = new List<Node>();
        if (Parent != null)
        {
            Depth = Parent.Depth + 1;
        }
    }

    public State State { get; set; }
    public int Depth { get; set; }
    public Node Parent { get; set; }
    public List<Node> Children { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null || !(obj is Node))
        {
            return false;
        }
        Node other = obj as Node;
        return State.Equals(other.State);
    }

    public Status GetStatus()
    {
        return State.GetStatus();
    }

    public int GetHeuristics(Turn currentTurn)
    {
        if (Children.Count == 0)
        {
            return State.GetHeuristics(currentTurn);
        }
        return Children[0].GetHeuristics(currentTurn);
    }
    public void SortChildrenMiniMax(Turn currentTurn, bool isCurrentPlayer = true)
    {
        foreach (Node node in Children)
        {
            node.SortChildrenMiniMax(currentTurn, !isCurrentPlayer);
        }
        if (isCurrentPlayer)
        {
            Children.Sort((x, y) => y.GetHeuristics(currentTurn).CompareTo(x.GetHeuristics(currentTurn)));
        }
        else
        {
            Children.Sort((x, y) => x.GetHeuristics(currentTurn).CompareTo(y.GetHeuristics(currentTurn)));
        }
    }
}
