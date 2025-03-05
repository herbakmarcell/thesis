using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOperator : Operator
{
    public TurnOperator(List<PlayerAction> playerActions)
    {
        this.playerActions = playerActions;
    }

    public List<PlayerAction> playerActions;

    public State Apply(State state)
    {
        if (state == null || !(state is StateRepresentation))
        {
            throw new Exception();
        }

        StateRepresentation newState = state.Clone() as StateRepresentation;
        foreach (PlayerAction playerAction in playerActions)
        {
            if (playerAction.actionType == ActionType.MOVE)
            {
                PlayerObject player = newState.ListPlayerObjects(newState.CurrentTurn == Turn.AI).Find(x => x.id == playerAction.playerId);
                newState.board[(int)player.position.x, (int)player.position.y] = new FieldObject("EMPTY", player.position);
                Vector2 newPos = ActionPosition(playerAction, player);
                newState.board[(int)newPos.x, (int)newPos.y] = player;
                player.position = newPos;
                //TODO move
                //MapFunctions.EntityMove(player.id, player.isAI,playerAction.actionPosition);
                
            }
            else
            {
                PlayerObject player = newState.ListPlayerObjects(newState.CurrentTurn == Turn.AI).Find(x => x.id == playerAction.playerId);
                Vector2 actionPos = ActionPosition(playerAction, player);
                PlayerObject target = newState.ListPlayerObjects(newState.CurrentTurn != Turn.AI).Find(x => x.position == actionPos);
                target.health -= player.attack;
                //TODO
            }
        }
        newState.ChangeTurn();

        return newState;
    }


    public bool IsApplicable(State state)
    {
        if (!isValidState(state)) return false;

        StateRepresentation stateRepresentation = state as StateRepresentation;
        bool illegalAction = false;
        foreach (PlayerAction playerAction in playerActions) 
        {
            if (playerAction.actionType == ActionType.MOVE)
            {
                illegalAction = !IsApplicableMove(stateRepresentation, playerAction);
            }
            else
            {
                illegalAction = !IsApplicableAttack(stateRepresentation, playerAction);
            }

            if (illegalAction) return false;
        }
        return true;
    }

    bool IsApplicableMove(StateRepresentation state, PlayerAction action)
    {
        PlayerObject player = state.ListPlayerObjects(state.CurrentTurn == Turn.AI).Find(x => x.id == action.playerId);

        return  IsOnBoard(action, player) &&
                IsCharacterThePlayer(state, player) &&
                IsEmpty(state, action, player) &&
                IsPlayerTurn(state, player);
    }

    bool IsApplicableAttack(StateRepresentation state, PlayerAction action)
    {
        PlayerObject player = state.ListPlayerObjects(state.CurrentTurn == Turn.AI).Find(x => x.id == action.playerId);

        return  IsOnBoard(action, player) &&
                IsCharacterThePlayer(state, player) &&
                IsOpposite(state, player, action) &&
                IsPlayerTurn(state, player);
    }

    Vector2 ActionPosition(PlayerAction playerAction, PlayerObject player)
    {
        switch (playerAction.actionDirection)
        {
            case ActionDirection.UP:
                return player.position + Vector2.up;
            case ActionDirection.DOWN:
                return player.position + Vector2.down;
            case ActionDirection.LEFT:
                return player.position + Vector2.left;
            case ActionDirection.RIGHT:
                return player.position + Vector2.right;
            default:
                return player.position;
        }
    }
    bool isValidState(State state)
    {
        return state != null && state is StateRepresentation;
    }
    bool IsOnBoard(PlayerAction playerAction, PlayerObject player)
    {
        Vector2 newPos = ActionPosition(playerAction, player);
        return newPos.x >= 0 && newPos.x < 10 && newPos.y >= 0 && newPos.y < 7;
    }
    bool IsCharacterThePlayer(StateRepresentation state, PlayerObject player)
    {
        FieldObject fieldObject = state.board[(int)player.position.x, (int)player.position.y];
        return fieldObject is PlayerObject p && p == player;
    }
    bool IsEmpty(StateRepresentation state, PlayerAction action, PlayerObject player)
    {
        Vector2 newPos = ActionPosition(action, player);
        FieldObject fieldObject = state.board[(int)newPos.x, (int)newPos.y];
        return fieldObject.id == "EMPTY";
    }
    bool IsOpposite(StateRepresentation state, PlayerObject player, PlayerAction action)
    {
        Vector2 newPos = ActionPosition(action, player);
        if (player.isAI)
        {
            return state.board[(int)newPos.x, (int)newPos.y].id.Contains("PLAYER");
        }
        return state.board[(int)newPos.x, (int)newPos.y].id.Contains("ENEMY");
    }
    bool IsPlayerTurn(StateRepresentation state, PlayerObject player)
    {
        return state.CurrentTurn == Turn.PLAYER && player.id.Contains("PLAYER") || state.CurrentTurn == Turn.AI && player.id.Contains("ENEMY");
    }
}

