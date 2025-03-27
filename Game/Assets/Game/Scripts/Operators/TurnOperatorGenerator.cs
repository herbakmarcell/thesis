using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class TurnOperatorGenerator : OperatorGenerator
{
    public List<Operator> Operators { get; }

    public TurnOperatorGenerator()
    {
        Operators = new List<Operator>();
        GeneratePlayerOperators();
        GenerateAIOperators();
    }

    public TurnOperatorGenerator(List<PlayerObject> playerObjects, List<PlayerObject> AIObjects)
    {
        Operators = new List<Operator>();
        GeneratePlayerOperators(playerObjects);
        GenerateAIOperators(AIObjects);
    }
    void GeneratePlayerOperators()
    {
        List<List<PlayerAction>> actions = new List<List<PlayerAction>>();
        for (int i = 0; i < Options.friendlyCount; i++)
        {
            List<PlayerAction> playerActionI = new List<PlayerAction>
            {
                new PlayerAction("PLAYER" + (i + 1), ActionType.MOVE, ActionDirection.UP),
                new PlayerAction("PLAYER" + (i + 1), ActionType.MOVE, ActionDirection.DOWN),
                new PlayerAction("PLAYER" + (i + 1), ActionType.MOVE, ActionDirection.LEFT),
                new PlayerAction("PLAYER" + (i + 1), ActionType.MOVE, ActionDirection.RIGHT),
                new PlayerAction("PLAYER" + (i + 1), ActionType.ATTACK, ActionDirection.UP),
                new PlayerAction("PLAYER" + (i + 1), ActionType.ATTACK, ActionDirection.DOWN),
                new PlayerAction("PLAYER" + (i + 1), ActionType.ATTACK, ActionDirection.LEFT),
                new PlayerAction("PLAYER" + (i + 1), ActionType.ATTACK, ActionDirection.RIGHT)
            };     
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
            //Debug.Log($"[PLAYER] {string.Join('x', lenghts)} = {lenghts.Aggregate(1, (acc, x) => acc * x)} \t Current: {Operators.Count}");
            for (int i = indexes.Count - 1; i >= 0; i--)
            {
                indexes[i]++;
                if (indexes[i] < lenghts[i] || i == 0) break;
                indexes[i] = 0;
            }
        }
    }

    void GeneratePlayerOperators(List<PlayerObject> playerObjects)
    {
        List<List<PlayerAction>> actions = new List<List<PlayerAction>>();
        foreach (var player in playerObjects)
        {
            List<PlayerAction> playerActionI = new List<PlayerAction>
            {
                new PlayerAction(player.id, ActionType.MOVE, ActionDirection.UP),
                new PlayerAction(player.id, ActionType.MOVE, ActionDirection.DOWN),
                new PlayerAction(player.id, ActionType.MOVE, ActionDirection.LEFT),
                new PlayerAction(player.id, ActionType.MOVE, ActionDirection.RIGHT),
                new PlayerAction(player.id, ActionType.ATTACK, ActionDirection.UP),
                new PlayerAction(player.id, ActionType.ATTACK, ActionDirection.DOWN),
                new PlayerAction(player.id, ActionType.ATTACK, ActionDirection.LEFT),
                new PlayerAction(player.id, ActionType.ATTACK, ActionDirection.RIGHT)
            };
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
            //Debug.Log($"[PLAYER] {string.Join('x', lenghts)} = {lenghts.Aggregate(1, (acc, x) => acc * x)} \t Current: {Operators.Count}");
            for (int i = indexes.Count - 1; i >= 0; i--)
            {
                indexes[i]++;
                if (indexes[i] < lenghts[i] || i == 0) break;
                indexes[i] = 0;
            }
        }
    }

    void GenerateAIOperators()
    {
        List<List<PlayerAction>> actions = new List<List<PlayerAction>>();
        for (int i = 0; i < Options.enemyCount; i++)
        {
            List<PlayerAction> playerActionI = new List<PlayerAction>
            {
                new PlayerAction("ENEMY" + (i + 1), ActionType.MOVE, ActionDirection.UP),
                new PlayerAction("ENEMY" + (i + 1), ActionType.MOVE, ActionDirection.DOWN),
                new PlayerAction("ENEMY" + (i + 1), ActionType.MOVE, ActionDirection.LEFT),
                new PlayerAction("ENEMY" + (i + 1), ActionType.MOVE, ActionDirection.RIGHT),
                new PlayerAction("ENEMY" + (i + 1), ActionType.ATTACK, ActionDirection.UP),
                new PlayerAction("ENEMY" + (i + 1), ActionType.ATTACK, ActionDirection.DOWN),
                new PlayerAction("ENEMY" + (i + 1), ActionType.ATTACK, ActionDirection.LEFT),
                new PlayerAction("ENEMY" + (i + 1), ActionType.ATTACK, ActionDirection.RIGHT)
            };
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
            //Debug.Log($"[AI] {string.Join('x', lenghts)} = {lenghts.Aggregate(1, (acc, x) => acc * x)} \t Current: {Operators.Count}");
            for (int i = indexes.Count - 1; i >= 0; i--)
            {
                indexes[i]++;
                if (indexes[i] < lenghts[i] || i == 0) break;
                indexes[i] = 0;
            }
        }
    }

    void GenerateAIOperators(List<PlayerObject> AIObjects)
    {
        List<List<PlayerAction>> actions = new List<List<PlayerAction>>();
        foreach (var ai in AIObjects)
        {
            List<PlayerAction> playerActionI = new List<PlayerAction>
            {
                new PlayerAction(ai.id, ActionType.MOVE, ActionDirection.UP),
                new PlayerAction(ai.id, ActionType.MOVE, ActionDirection.DOWN),
                new PlayerAction(ai.id, ActionType.MOVE, ActionDirection.LEFT),
                new PlayerAction(ai.id, ActionType.MOVE, ActionDirection.RIGHT),
                new PlayerAction(ai.id, ActionType.ATTACK, ActionDirection.UP),
                new PlayerAction(ai.id, ActionType.ATTACK, ActionDirection.DOWN),
                new PlayerAction(ai.id, ActionType.ATTACK, ActionDirection.LEFT),
                new PlayerAction(ai.id, ActionType.ATTACK, ActionDirection.RIGHT)
            };
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
            //Debug.Log($"[AI] {string.Join('x', lenghts)} = {lenghts.Aggregate(1, (acc, x) => acc * x)} \t Current: {Operators.Count}");
            for (int i = indexes.Count - 1; i >= 0; i--)
            {
                indexes[i]++;
                if (indexes[i] < lenghts[i] || i == 0) break;
                indexes[i] = 0;
            }
        }
    }
}
