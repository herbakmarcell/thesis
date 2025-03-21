using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMaxWithAlphaBetaPruning : Solver
{
    public int Depth { get; set; }

    public MiniMaxWithAlphaBetaPruning(OperatorGenerator operatorGenerator, int depth) : base(operatorGenerator)
    {
        Depth = depth;
    }


    public override State NextMove(State state)
    {
        Node currentNode = new Node(state);

        ExtendNode(currentNode, int.MinValue, int.MaxValue, currentNode.State.CurrentTurn);
        //Debug.Log(currentNode.Children[0].State == null);
        Debug.Log((currentNode.Children[0].State as StateRepresentation).ListPlayerObjects(true)[0].position.x);
        Debug.Log((currentNode.Children[0].State as StateRepresentation).ListPlayerObjects(true)[0].position.y);
        return currentNode.Children[0].State;
    }

    private int ExtendNode(Node node, int alpha, int beta, Turn currentTurn, bool currentPlayer = true)
    {
        if (node.State.GetStatus() != Status.PLAYING || node.Depth >= Depth) return node.GetHeuristics(currentTurn);

        int v = currentPlayer ? int.MinValue : int.MaxValue;

        foreach (Operator op in Operators)
        {
            
            if (op.IsApplicable(node.State))
            {
                State newState = op.Apply(node.State);
                Node newNode = new Node(newState, node);
                node.Children.Add(newNode);
                node.SortChildrenMiniMax(currentTurn, currentPlayer);

                if (currentPlayer)
                {
                    v = Mathf.Max(v, ExtendNode(newNode, alpha, beta, currentTurn, !currentPlayer));
                    if (v > beta) return v;
                    alpha = Mathf.Max(alpha, v);
                }
                else
                {
                    v = Mathf.Min(v, ExtendNode(newNode, alpha, beta, currentTurn, !currentPlayer));
                    if (v < alpha) return v;
                    beta = Mathf.Min(beta, v);
                }

            }
        }

        return v;
    }
}
