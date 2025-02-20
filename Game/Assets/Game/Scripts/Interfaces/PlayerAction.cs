using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ActionType
{
    MOVE, ATTACK
}
public class PlayerAction
{
    public string playerId;
    public ActionType action;
    public Vector2 actionPosition;

    public PlayerAction(string playerId, ActionType action, Vector2 actionPosition)
    {
        this.playerId = playerId;
        this.action = action;
        this.actionPosition = actionPosition;
    }
    public PlayerAction()
    {
        
    }
}
