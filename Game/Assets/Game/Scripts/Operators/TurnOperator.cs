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
            if (playerAction.action == ActionType.MOVE)
            {
                PlayerObject player = newState.ListPlayerObjects(newState.CurrentTurn == Turn.AI).Find(x => x.id == playerAction.playerId);
                newState.board[(int)player.position.x, (int)player.position.y] = new FieldObject("EMPTY", player.position);
                newState.board[(int)playerAction.actionPosition.x, (int)playerAction.actionPosition.y] = player;
                player.position = playerAction.actionPosition;
                //TODO move
                MapFunctions.EntityMove(player.id, player.isAI,playerAction.actionPosition);
                
            }
            else
            {
                PlayerObject player = newState.ListPlayerObjects(newState.CurrentTurn == Turn.AI).Find(x => x.id == playerAction.playerId);
                PlayerObject target = newState.ListPlayerObjects(newState.CurrentTurn != Turn.AI).Find(x => x.position == playerAction.actionPosition);
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
            if (playerAction.action == ActionType.MOVE)
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

        return  IsOnBoard(action) &&
                IsNewPosition(player, action) &&
                IsOneSquare(player, action) &&
                IsCharacterThePlayer(state, player) &&
                IsEmpty(state, action) &&
                IsPlayerTurn(state, player);
    }

    bool IsApplicableAttack(StateRepresentation state, PlayerAction action)
    {
        PlayerObject player = state.ListPlayerObjects(state.CurrentTurn == Turn.AI).Find(x => x.id == action.playerId);

        return  IsOnBoard(action) &&
                IsNewPosition(player, action) &&
                IsOneSquare(player, action) &&
                IsCharacterThePlayer(state, player) &&
                IsOpposite(state, player, action) &&
                IsPlayerTurn(state, player);
    }

    bool isValidState(State state)
    {
        return state != null && state is StateRepresentation;
    }
    bool IsOnBoard(PlayerAction playerAction)
    {
        return playerAction.actionPosition.x >= 0 && playerAction.actionPosition.x <= 9 && playerAction.actionPosition.y >= 0 && playerAction.actionPosition.y <= 6;
    }
    bool IsNewPosition(PlayerObject player, PlayerAction action)
    {
        return player.position.x != action.actionPosition.x && player.position.y != action.actionPosition.y;
    }
    bool IsOneSquare(PlayerObject player, PlayerAction action)
    {
        return (Math.Abs(player.position.x - action.actionPosition.x) == 1) && (Math.Abs(player.position.y - action.actionPosition.y) == 1);
    }
    bool IsCharacterThePlayer(StateRepresentation state, PlayerObject player)
    {
        FieldObject fieldObject = state.board[(int)player.position.x, (int)player.position.y];
        return fieldObject is PlayerObject p && p == player;
    }
    bool IsEmpty(StateRepresentation state, PlayerAction action)
    {
        FieldObject fieldObject = state.board[(int)action.actionPosition.x, (int)action.actionPosition.y];
        return fieldObject.id == "EMPTY";
    }
    bool IsOpposite(StateRepresentation state, PlayerObject player, PlayerAction action)
    {
        if (player.isAI)
        {
            return state.board[(int)action.actionPosition.x, (int)action.actionPosition.y].id.Contains("PLAYER");
        }
        return state.board[(int)action.actionPosition.x, (int)action.actionPosition.y].id.Contains("ENEMY");
    }
    bool IsPlayerTurn(StateRepresentation state, PlayerObject player)
    {
        return state.CurrentTurn == Turn.PLAYER && player.id.Contains("PLAYER") || state.CurrentTurn == Turn.AI && player.id.Contains("ENEMY");
    }
}

