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
