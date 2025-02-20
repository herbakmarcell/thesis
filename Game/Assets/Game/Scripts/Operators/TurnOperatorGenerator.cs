using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnOperatorGenerator : OperatorGenerator
{
    public List<Operator> Operators { get; }

    public TurnOperatorGenerator()
    {
        Operators = new List<Operator>();
        List<List<PlayerAction>> actions = new List<List<PlayerAction>>();
        for (int i = 0; i < Options.friendlyCount; i++)
        {
            List<PlayerAction> playerActionI = new List<PlayerAction>();
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    playerActionI.Add(new PlayerAction("PLAYER" + (i + 1), ActionType.MOVE, new Vector2(x, y)));
                    playerActionI.Add(new PlayerAction("PLAYER" + (i + 1), ActionType.ATTACK, new Vector2(x, y)));
                }
            }
            actions.Add(playerActionI);
        }
        List<int> lenghts = actions.Select(x => x.Count).ToList();
        List<int> indexes = actions.Select(x => 0).ToList();
        while (indexes[0] < lenghts[0])
        {
            List<PlayerAction> playerActions = new List<PlayerAction>();
            for (int i = 0; i < actions.Count; i++)
            {
                playerActions.Add(actions[i][indexes[i]]);
            }
            Operators.Add(new TurnOperator(playerActions));
            Debug.Log($"{string.Join('x',lenghts)} = {lenghts.Aggregate(1,(acc,x) => acc*x)} \t Current: {Operators.Count}");
            for (int i = indexes.Count - 1; i >= 0; i--)
            {
                indexes[i]++;
                if (indexes[i] < lenghts[i] || i == 0) break;
                indexes[i] = 0;
            }
        }








        //for (int i = 0; i < actions[0].Count; i++)
        //{
        //    for (int j = 0; j < actions[1].Count; j++)
        //    {
        //        for (int k = 0; k < actions[2].Count; k++)
        //        {
        //            Operators.Add(new TurnOperator(
        //                new List<PlayerAction> { 
        //                    actions[0][i], 
        //                    actions[1][j], 
        //                    actions[2][k] 
        //                }));
        //        }
        //    }
        //}








        //for (int i = 0; i < Options.enemyCount; i++)
        //{

        //}
    }
}
