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
