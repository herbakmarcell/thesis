using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ActionType
{
    MOVE, ATTACK
}
public enum ActionDirection
{
    UP, DOWN, LEFT, RIGHT
}
public class PlayerAction
{
    public string playerId;
    public ActionType actionType;
    public ActionDirection actionDirection;

    public PlayerAction(string playerId, ActionType action, ActionDirection actionDirection)
    {
        this.playerId = playerId;
        this.actionType = action;
        this.actionDirection = actionDirection;
    }
    public PlayerAction()
    {
        
    }
}
