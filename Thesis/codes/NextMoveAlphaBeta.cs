public override State NextMove(State state)
{
    Node currentNode = new Node(state);
    ExtendNode(currentNode, int.MinValue, int.MaxValue, currentNode.State.CurrentTurn);
    return currentNode.Children[0].State;
}