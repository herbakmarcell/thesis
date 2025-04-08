using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

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
            if (newState.ListAllPlayerObjects().Find(x => x.id == playerAction.playerId) == null)
            {
                continue;
            }



            if (playerAction.actionType == ActionType.MOVE)
            {
                PlayerObject player = newState.ListAllPlayerObjects().Find(x => x.id == playerAction.playerId).Clone() as PlayerObject;
                
                newState.board[(int)player.position.y, (int)player.position.x] = new FieldObject("EMPTY", player.position);

                Vector2 newPos = ActionPosition(playerAction, player);
                newState.board[(int)newPos.y, (int)newPos.x] = player;
                player.position = newPos;
            }
            else
            {


                PlayerObject player = newState.ListAllPlayerObjects().Find(x => x.id == playerAction.playerId).Clone() as PlayerObject;
                Vector2 actionPos = ActionPosition(playerAction, player);

                List<PlayerObject> playerObjects;
                if (playerAction.playerId.Contains("PLAYER")) playerObjects = newState.ListPlayerObjects(true);
                else playerObjects = newState.ListPlayerObjects(false);

                PlayerObject target = playerObjects.Find(x => x.position == actionPos);
                if (target == null)
                {
                    newState.board[(int)player.position.y, (int)player.position.x] = new FieldObject("EMPTY", player.position);
                    Vector2 newPos = ActionPosition(playerAction, player);
                    newState.board[(int)newPos.y, (int)newPos.x] = player;
                    player.position = newPos;
                } 
                else
                {
                    PlayerObject targetClone = target.Clone() as PlayerObject;
                    targetClone.health -= player.attack;
                    if (targetClone.health <= 0)
                    {
                        newState.board[(int)targetClone.position.y, (int)targetClone.position.x] = new FieldObject("EMPTY", targetClone.position);
                    }
                    else
                    {
                        newState.board[(int)targetClone.position.y, (int)targetClone.position.x] = targetClone;
                    }
                }  
            }
        }
        //playerActions.Clear();
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
            PlayerObject player = stateRepresentation.ListAllPlayerObjects().Find(x => x.id == playerAction.playerId);
            if (player == null)
            {
                continue;
            }

            if (playerAction.actionType == ActionType.MOVE)
            {
                illegalAction = !IsApplicableMove(stateRepresentation, playerAction);

            }
            else if (playerAction.actionType == ActionType.ATTACK)
            {
                //Vector2 actionPos = ActionPosition(playerAction, player);
                //if (stateRepresentation.ListAllPlayerObjects().Find(x => x.position == actionPos) == null) return false;
                illegalAction = !IsApplicableAttack(stateRepresentation, playerAction);
            } 

            if (illegalAction) return false;
        }
        if (HasSamePosition(state)) return false;
        return true;
    }

    bool IsApplicableMove(StateRepresentation state, PlayerAction action)
    {
        PlayerObject player = state.ListAllPlayerObjects().Find(x => x.id == action.playerId);

        return  isValidState(state) &&
                IsOnBoard(action, player) &&
                IsCharacterThePlayer(state, player) &&
                IsEmpty(state, action, player) &&
                IsPlayerTurn(state, player);
    }

    bool IsApplicableAttack(StateRepresentation state, PlayerAction action)
    {
        PlayerObject player = state.ListAllPlayerObjects().Find(x => x.id == action.playerId);

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

    bool HasSamePosition(State state)
    {
        List<PlayerObject> playerObjects = null;
        if ((state as StateRepresentation).CurrentTurn == Turn.AI)
        {
            playerObjects = (state as StateRepresentation).ListAllPlayerObjects().Where(x => x.isAI).ToList();
        }
        else if ((state as StateRepresentation).CurrentTurn == Turn.PLAYER)
        {
            playerObjects = (state as StateRepresentation).ListAllPlayerObjects().Where(x => !x.isAI).ToList();
        }
        List<PlayerObject> playerObjects2 = new List<PlayerObject>();
        foreach (var item in playerObjects)
        {
            PlayerObject player = item.Clone() as PlayerObject;
            foreach (var action in playerActions)
            {
                if(action.playerId == player.id && action.actionType == ActionType.MOVE) 
                    player.position = ActionPosition(playerActions.Find(x => x.playerId == player.id), player);
            }
            
            playerObjects2.Add(player);
        }
        List<Vector2> positions = new List<Vector2>();
        foreach (PlayerObject playerObject in playerObjects2)
        {
            if (positions.Contains(playerObject.position))
            {
                return true;
            }
            positions.Add(playerObject.position);
        }
        return false;
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
        FieldObject fieldObject = state.board[(int)player.position.y, (int)player.position.x];
        return fieldObject is PlayerObject p && p == player;
    }
    bool IsEmpty(StateRepresentation state, PlayerAction action, PlayerObject player)
    {
        Vector2 newPos = ActionPosition(action, player);
        FieldObject fieldObject = state.board[(int)newPos.y, (int)newPos.x];
        return fieldObject.id == "EMPTY";
    }
    bool IsOpposite(StateRepresentation state, PlayerObject player, PlayerAction action)
    {
        Vector2 newPos = ActionPosition(action, player);
        if (player.isAI)
        {
            return state.board[(int)newPos.y, (int)newPos.x].id.Contains("PLAYER");
        }
        return state.board[(int)newPos.y, (int)newPos.x].id.Contains("ENEMY");
    }
    bool IsPlayerTurn(StateRepresentation state, PlayerObject player)
    {
        return (state.CurrentTurn == Turn.PLAYER && player.id.Contains("PLAYER")) || (state.CurrentTurn == Turn.AI && player.id.Contains("ENEMY"));
    }
}

