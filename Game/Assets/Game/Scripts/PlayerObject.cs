using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : FieldObject, ICloneable
{
    public int health;
    public int attack;
    public bool isAI;

    public PlayerObject(string id, Vector2 position, int health, int attack, bool isAI) : base(id,position)
    {
        this.health = health;
        this.attack = attack;
        this.isAI = isAI;
    }

    public override object Clone()
    {
        return new PlayerObject(id, new Vector2(position.x, position.y), health, attack, isAI);
    }
}
